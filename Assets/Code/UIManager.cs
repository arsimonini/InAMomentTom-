using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public PlayerUICanvas playerUICanvas;
    public UpgradeUICanvas upgradeUICanvas;

    public PlayerUICanvas GetPlayerUICanvas(){ 
        return playerUICanvas;
    }

    public UpgradeUICanvas GetUpgradeUICanvas(){
        return upgradeUICanvas;
    }

    public void SetUIModeLaunch(){
        playerUICanvas.gameObject.SetActive(true);
        upgradeUICanvas.gameObject.SetActive(false);
    }

    public void SetUIModeUpgrade(){
        playerUICanvas.gameObject.SetActive(false);
        upgradeUICanvas.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO - This shouldn't just default here
        SetUIModeLaunch();
    }
}
