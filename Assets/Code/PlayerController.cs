using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Get a reference to the RB on the object
    private Rigidbody rb;

    // Public spped modifier variable
    public float moveSpeed = 1;

    // Initial transform to reset to and calc from
    public Transform startTransform;
    private float prevX;

    // GameManager object
    private GameManager GM;

    // Drag multipler effects how much drag is applied on movement
    // Scalar value to the axisInput (1 is all the way pressed down)
    public float dragModifier = 0.3f;

    // Add a flag to see if the player has hit the ground this run
    public bool hasHitGround = false;
    // Threshold where a player gets reset from velocity
    public float resetPlayerVelocityThreshold = 0.01f;
    // Reset backup delay in seconds
    public float resetDelaySeconds = 3.0f;

    // Async routine to reset the player 
    private Coroutine resetCoroutine;

    // Current bounce cooldown timer
    private float currentBounceCooldownTimer = 0;
    private bool isOnBounceCooldown = false;

    public GameObject briefcase;
    private bool isBriefcaseOn = false;
    public float briefcaseOffset = 0.1f;
    private float bounceForce = 10f;
    [Tooltip("Vertical distance threshold to consider the player as 'in contact' with the Respawn object.")]
    public float contactThreshold = 0.5f;
    private Transform respawnTransform;

    void Start()
    {
        if(!TryGetComponent<Rigidbody>(out rb)){
            Debug.LogError("PlayerController can't find a RigidBody!");
        }

        GetUpgradeValues();

        ResetPlayerTransform();

        respawnTransform = GameObject.FindGameObjectsWithTag("Respawn")[0].transform;

        if (briefcase != null)
        {
            briefcase.GetComponent<Renderer>().enabled = false;
            isBriefcaseOn = false;
        }

    }

    public void GetUpgradeValues(){
        dragModifier = GM.GetUpgradeManager().getMovementDragModifier();
        bounceForce = GM.GetUpgradeManager().getMovementBounceForce();
    }

    private void ResetPlayerTransform(){
        // Apply resets
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    private void ResetPlayerRigidbody(){
        // Reset rigidbody data
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void CalculateDistance(){
        // Apply distance-dependent calculations
        GM.GetUpgradeManager().TravelDistance(MathF.Abs(transform.position.x - prevX));
        // Update the previous X position to the current
        prevX = transform.position.x;
    }

    void Update()
    {
        // If the player is launched collect money
        if(GM.GetGameState() == GameState.Launched){
            CalculateDistance();

            HandleBriefcaseUpdate();
        
            
        
        }

        
    }

    private void HandleBriefcaseUpdate()
    {
        bool canBounce = true;

        
        if(isOnBounceCooldown){
            canBounce = false;
            currentBounceCooldownTimer += Time.deltaTime;
            if( currentBounceCooldownTimer >= GM.GetUpgradeManager().getBounceCooldown()){
                currentBounceCooldownTimer = 0;
                isOnBounceCooldown = false;
            }
        }
        
        if(canBounce){
            // Basic briefcase input
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (briefcase != null)
                {
                    briefcase.GetComponent<Renderer>().enabled = true;
                    isBriefcaseOn = true;
                }
            }
            
            // Update the briefcase's position if it's active.
            if (briefcase != null && isBriefcaseOn)
            {
                // Position directly below the player (ign player rotation)
                Vector3 targetPosition = transform.position + Vector3.down * briefcaseOffset;
                briefcase.transform.position = targetPosition;
                // Reset the briefcase's rotation so it always stays upright
                briefcase.transform.rotation = Quaternion.identity;
            }

            // When the key is released, deactivate the briefcase.
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (briefcase != null)
                {
                    briefcase.GetComponent<Renderer>().enabled = false;
                    isBriefcaseOn = false;
                    isOnBounceCooldown = true;
                }
            }
        }

        
    }

    void FixedUpdate()
    {
        // Safety check conf. RB exist
        if(rb != null && GM.GetGameState() == GameState.Launched){
            // Apply movement logic
            ApplyMove();
            // Check for player stopping
            CheckIsPlayerStopped();

            // Check for bounce
            CheckForBriefcaseBounce();
        }
    }


    public void CheckForBriefcaseBounce(){
        if(isBriefcaseOn){
            float distanceY = transform.position.y - respawnTransform.position.y;
            if(distanceY <= contactThreshold){
                if(rb.velocity.y < 0){
                    rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
                }
            }
        }
    }
    // Game Manager setting function, allows for the manager to get set on find
    public void SetGameManager(GameManager gameManager){
        GM = gameManager;
    }

    private void ApplyMove(){
        // Basic axis input movement
        float axisInput = Input.GetAxis("Horizontal");
        Vector3 moveVector = transform.right * axisInput;
        rb.MovePosition(transform.position + moveVector * Time.fixedDeltaTime * moveSpeed);   

        // Cute way to add drag relative to input ( 0 - 1)
        rb.drag = MathF.Abs(axisInput * dragModifier);
    }

    private void CheckIsPlayerStopped(){
        // If the player has a low enough velocity and has hit the ground reset
        if(rb.velocity.magnitude <= resetPlayerVelocityThreshold && hasHitGround){
            GM.ResetPlayer();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Respawn"){
            hasHitGround = true;
            // Begin the reset timer
            StartResetTimer();
        }
    }

    public void ResetPlayer(){
        // Reset hasHitGround flag
        hasHitGround = false;
        // Reset RB
        ResetPlayerRigidbody();
        // Reset Transform
        ResetPlayerTransform();

        // Stop all coroutines
        StopAllCoroutines();
    }

    public void StartResetTimer()
    {
        // Confirm the reset timer hasn't already started
        if (resetCoroutine == null)
        {
            resetCoroutine = StartCoroutine(DelayedReset());
        }
    }

    private IEnumerator DelayedReset()
    {
        // Wait X async, then reset
        yield return new WaitForSeconds(resetDelaySeconds);
        GM.ResetPlayer();
        resetCoroutine = null;
    }
    
}
