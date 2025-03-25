using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchManager : MonoBehaviour
{

    private PlayerUICanvas playerUICanvas;

    private float currentPower = 0;
    private float currentAngle = 0;

    [Tooltip("Controls the rate the power changes")]
    public float powerChangeSpeed = 1;
    
    [Tooltip("Controls the rate the angle changes")]
    public float angleChangeSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
       InitGUI();
    }

    void Update(){

        UpdatePower();
        UpdateAngle();

        if(playerUICanvas != null){
            UpdateGUI();
        }
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
