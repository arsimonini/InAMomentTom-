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

    [Tooltip("Controls the rate the power changes")]
    public float powerChangeSpeed = 1;
    
    [Tooltip("Controls the rate the angle changes")]
    public float angleChangeSpeed = 1;

    // Tracks the current launch state
    public LaunchState cLaunchState = LaunchState.Idle;
    

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Setter for launch state, readable
    private void SetLaunchState(LaunchState newState){
        cLaunchState = newState;
    }

    private void UpdatePower(){
        currentPower = currentPower + (Time.deltaTime * powerChangeSpeed);
    }

    private void UpdateAngle(){
        currentAngle = currentAngle + (Time.deltaTime * angleChangeSpeed);
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
