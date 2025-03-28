using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LaunchState{
    Idle,
    Power,
    Angle,
    Launched
}

public class LaunchManager : MonoBehaviour
{

    private PlayerUICanvas playerUICanvas;

    private float currentPower = 0;
    private float currentAngle = 0;

    // Two available modifiers for launch distance / power
    [Tooltip("Controls the horizontal power applied")]

    public float horizontalLaunchPowerModifier = 1;
    [Tooltip("Controls the vertical power applied")]
    public float verticalLaunchPowerModifier = 1;

    [Tooltip("Controls the rate the power changes")]
    public float powerChangeSpeed = 1;
    
    [Tooltip("Controls the rate the angle changes")]
    public float angleChangeSpeed = 1;

    // Tracks the current launch state
    public LaunchState cLaunchState = LaunchState.Idle;

    // Scrpit will find the player object in the scene with tag Player
    private GameObject playerObject;
    

    // Start is called before the first frame update
    void Start()
    {
        // Searches for the first instance of a Player gameobject w/ that tag
        playerObject = GameObject.FindGameObjectsWithTag("Player")[0];
        InitGUI();
    }

    void Update(){

        HandleUpdateInput();

        if(playerUICanvas != null){
            UpdateGUI();
        }
    }

    // Takes input management into its own class
    // THIS USES THE "ActionKey" INPUT BUTTON
    // THIS CAN BE MODIFIED IN THE:
    // Edit -> ProjectSettings -> Input Manager -> Axes

    // Keep in mind this was built with the basic Unity input manager
    // This can (and should) be refactored to use the new Unity input manager package if the project gets more complex
    // Due to there only being a couple of inputs for a small project this input manager was deemed appropriate
    private void HandleUpdateInput(){

        if(cLaunchState == LaunchState.Idle){

            if(Input.GetButtonDown("ActionKey")){
                // If the action key gets pressed it'll start the process
                TransitionToPower();
            }

        }else if(cLaunchState == LaunchState.Power){

            if(Input.GetButton("ActionKey")){
                // If the key is being held, update POWER
                UpdatePower();
            }

            if(Input.GetButtonUp("ActionKey")){
                // Once the action key is up, move on to handling angle
                TransitionToAngle();
            }

        }else if(cLaunchState == LaunchState.Angle){

            if(Input.GetButton("ActionKey")){
                // If the key is being held, update ANGLE
                UpdateAngle();
            }

            if(Input.GetButtonUp("ActionKey")){
                // Once the action key is up, move on to handling angle
                TransitionToLaunched();
            }

        }else if(cLaunchState == LaunchState.Launched){
            // No launch input is handled
        }
    }

    // Functions for each primary transition
    private void TransitionToPower(){
        // Much more can be added
        // Place to add visual or UI effects
        Debug.Log("Transitioned to power state!");
        // Set the new launch state
        SetLaunchState(LaunchState.Power);
    }

    private void TransitionToAngle(){
        // Much mroe can be added
        // ... can add visual or UI effects
        Debug.Log("Transition to Angle state!");
        // Set new launch state
        SetLaunchState(LaunchState.Angle);

    }

    private void TransitionToLaunched(){
        // Set the new launch state (launch!)
        SetLaunchState(LaunchState.Launched);

        // Launch!
        LaunchPlayer();
    }

    private void LaunchPlayer(){
        // LAUNCH!
        Debug.Log("Player launched with power: " + currentPower + " and angle: " + currentAngle);
        Debug.DrawLine(playerObject.transform.position, playerObject.transform.position + CalculateAngleForce(), Color.black, 5f);
        if(playerObject != null){
            playerObject.GetComponent<Rigidbody>().AddRelativeForce(CalculateAngleForce());
        }
    }

    // Calculates force Vector to be applied to object
    private Vector3 CalculateAngleForce(){
        float angleDegrees = (currentAngle * 180f);
        float angleRad = angleDegrees * Mathf.Deg2Rad;
        Vector3 retValue = new Vector3(
            (Mathf.Sin(angleRad) * currentPower) * horizontalLaunchPowerModifier,
            (Mathf.Cos(angleRad) * currentPower) * verticalLaunchPowerModifier,
            0f
            );
        return retValue;
    }

    // Setter for launch state, readable
    private void SetLaunchState(LaunchState newState){
        cLaunchState = newState;
    }

    private void UpdatePower(){
        currentPower = MathF.Min(currentPower + (Time.deltaTime * powerChangeSpeed), 1f);

        // Returns power to 0 to loop
        if(currentPower == 1f){
            currentPower = 0;
        }
    }

    private void UpdateAngle(){
        currentAngle = MathF.Min(currentAngle + (Time.deltaTime * angleChangeSpeed), 1f);
    }

    private void UpdateGUI(){
        playerUICanvas.setPowerSliderValue(currentPower);
        playerUICanvas.setAngleSliderValue(currentAngle);
    }

    private void InitGUI(){
        GameObject ui_Canvas = GameObject.FindWithTag("UI_Canvas");
        if(ui_Canvas != null){
            playerUICanvas = ui_Canvas.GetComponent<PlayerUICanvas>();
        }else{
            // Debug statement can be removed later
            Debug.LogError("GUI Failed to Initialize");
        }
    }
}
