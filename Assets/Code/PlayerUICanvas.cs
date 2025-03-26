using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvas : MonoBehaviour
{

    // Keep the UI interactions in this class so other things don't need refs to in-UI gameobjects
    public Slider powerSlider;
    public Slider angleSlider;

    // Reference to a debug text, can be removed later
    public TextMeshProUGUI debugText;

    // Getters aren't super necessary as of now esp. bc this is a debug
    public float getPowerSliderValue(){
        return powerSlider.value;
    }
    
    public float getAngleSliderValue(){
        return angleSlider.value;
    }

    // These setters are used primarily for now (Week 1)
    public void setPowerSliderValue(float val){
        powerSlider.value = val;
        UpdateDebugUI();
    }

    public void setAngleSliderValue(float val){
        angleSlider.value = val;
        UpdateDebugUI();
    }

    private void UpdateDebugUI(){
        if (debugText != null){
            debugText.text = "Power: " + powerSlider.value + " \nAngle: " + angleSlider.value;
        }
    }


}
