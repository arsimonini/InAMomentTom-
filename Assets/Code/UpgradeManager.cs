using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    /*
            -- Sprint 1
            Increase Launch Power
            Decrease Air Resistance (Side Movement)
            Increase Speedometer Limit (Forward Movement)
            Increase Revenue Gain
            Stuffed Suitcase - Unlocks Suitcase Bouncing, then increase Bounce potency
             Increase Air Movement


            ----
            Coffee - Unlocks Caffeine Boost Mode, then increases how much Coffee is in cup per launch
            Decrease Ragdolling
            Decrease Bounce Slow Down
            Headphones - Unlocks sick tunes to play when launching plus radio exerts
            Decrease Police Car Potency
            Decrease Lemonade Stand 
            Increase Manhole Potency
            Increase Drone Potency
            Increase Bubblegum Potency
            Increase Balloon Potency
            Increase Car Potency
    */

    public float launchPowerModifierUpgrade = 1f;
    public float movementDragModifier = 1f;
    public float speedometerLimit = 150f;
    public float incomeModifier = 1f;

    // Economy variables start
    public float currentMoney = 0;

    // Boucne mod
    public float bounceModifier = 1f;

    // Briefcase Bounce Cooldown
    public float bounceCooldown = 1f;
    // Briefcase Bounce Lifetime
    public float bounceLength = 0.3f;

    // Upgrades start
    public List<UpgradeObj> upgrades;

    private GameManager GM;

    public float getBounceCooldown(){
        return bounceCooldown;
    }

    public float getBriefcaseMaxDuration(){
        return bounceLength;
    }

    public void TravelDistance(float distance){
        currentMoney += (distance * incomeModifier);
        UpdateUIMoney();
    }

    public void UpdateUIMoney(){
        GM.GetUIManager().GetPlayerUICanvas().setMoneyText(currentMoney);
        GM.GetUIManager().GetUpgradeUICanvas().setMoneyText(currentMoney);
    }

    public float getMovementBounceForce(){
        return bounceModifier;
    }

    public float getLaunchPowerModifier(){
        return launchPowerModifierUpgrade;
    }

    public float getMovementDragModifier(){
        return movementDragModifier;
    }

    // Game Manager setting function, allows for the manager to get set on find
    public void SetGameManager(GameManager gameManager){
        GM = gameManager;
    }

    // Initialize the upgrades UI
    private void InitializeUI(){
        UpgradeUICanvas uiCanvas = GM.GetUIManager().GetUpgradeUICanvas();
        foreach (UpgradeObj upgradeObj in upgrades){
            uiCanvas.CreateUpgradeButton(upgradeObj, this);
        }
        
    }

    public void UpgradeField(UpgradeObj upgradeObj){
        string upgradeID = upgradeObj.upgradeID;
        float currentCost = upgradeObj.GetCostAtLevel(upgradeObj.GetCurrentLevel());
        if(currentMoney < currentCost){
            return;
        }else{
            currentMoney -= currentCost;
            upgradeObj.Upgrade();
            UpdateUIMoney();
        }

        switch (upgradeID){
            case "LAUNCH_POWER":
                // Standard launch power upgrade
                launchPowerModifierUpgrade += 200f;
                break;
            case "DRAG_MOD":
                // Decreases the drag modifier
                movementDragModifier -= 0.002f;
                break;
            case "BOUNCE_MOD":
                // Adds some power to the bounce
                bounceModifier += 10f;
                break;
            case "COOLDOWN":
                // Cooldown decrease
                bounceCooldown -= 1f;
                break;
            case "BOUNCE_LIFESPAN":
                bounceLength += 0.5f;
                break;
            case "MONEY":
                // Income multipler (1x, 1.5x, 2x)
                incomeModifier += 0.5f;
                break;
            default:
                break;
        }

        GM.ApplyUpgrades();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
