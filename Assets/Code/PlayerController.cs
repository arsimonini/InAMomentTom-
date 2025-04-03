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

    // GameManager object
    private GameManager GM;

    // Drag multipler effects how much drag is applied on movement
    // Scalar value to the axisInput (1 is all the way pressed down)
    public float dragModifier = 0.3f;

    // Add a flag to see if the player has hit the ground this run
    public bool hasHitGround = false;
    // Threshold where a player gets reset from velocity
    public float resetPlayerVelocityThreshold = 0.01f;
    void Start()
    {
        if(!TryGetComponent<Rigidbody>(out rb)){
            Debug.LogError("PlayerController can't find a RigidBody!");
        }

        dragModifier = GM.GetUpgradeManager().getMovementDragModifier();

        ResetPlayerTransform();

    }

    private void ResetPlayerTransform(){
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
    }

    private void ResetPlayerRigidbody(){
        // Reset rigidbody data
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void PrintDistance(){
        Debug.Log("" + (transform.position.x - startTransform.position.x));
    }

    void Update()
    {
        //PrintDistance();
    }

    void FixedUpdate()
    {
        // Safety check conf. RB exist
        if(rb != null){
            // Apply movement logic
            ApplyMove();
            // Check for player stopping
            CheckIsPlayerStopped();
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
        }
    }

    public void ResetPlayer(){
        // Reset hasHitGround flag
        hasHitGround = false;
        // Reset RB
        ResetPlayerRigidbody();
        // Reset Transform
        ResetPlayerTransform();
    }
}
