using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeUICanvas : MonoBehaviour
{

    public TMP_Text moneyText;

    public Image cUpgradeSprite;
    public TMP_Text cUpgradeTitle;
    public TMP_Text cUpgradeDescription;

    public Button buttonUpgrade;

    public Transform upgradeButtonContainerPanel;

    public GameObject upgradeButtonPrefab;

    public List<GameObject> upgradeButtons = new List<GameObject>();

    public void setMoneyText(float newMoney){
        moneyText.text = ("$" + newMoney.ToString("0"));
    }

    void Start()
    {
        buttonUpgrade.gameObject.SetActive(false);
    }

    public void RefreshAllUIElements(){
        foreach(GameObject upgradeButton in upgradeButtons){
            UpgradeObj upgrade = upgradeButton.GetComponent<UpgradeButton>().assignedUpgrade;
            upgradeButton.gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Lvl. " + upgrade.currentLevel + "/" + upgrade.numLevels;
        }
    }

    public void CreateUpgradeButton(UpgradeObj upgrade, UpgradeManager upgradeManager){
        // Create a new empty upgrade button to populate
        GameObject newButton = Instantiate(upgradeButtonPrefab, upgradeButtonContainerPanel);
        // Populate base icon and onclick event
        newButton.GetComponent<Image>().sprite = upgrade.icon;
        newButton.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Lvl. " + upgrade.GetCurrentLevel() + "/" + upgrade.numLevels;
        newButton.GetComponent<Button>().onClick.AddListener(() => UpdateCurrentUpgradeUI(upgrade, upgradeManager));
        newButton.GetComponent<UpgradeButton>().assignedUpgrade = upgrade;
        upgradeButtons.Add(newButton);
    }

    public void UpdateCurrentUpgradeUI(UpgradeObj upgradeObj, UpgradeManager upgradeManager){
        // Update the info pane with relevant info on click
        cUpgradeSprite.sprite = upgradeObj.icon;
        cUpgradeTitle.text = upgradeObj.upgradeName;
        cUpgradeDescription.text = upgradeObj.upgradeDescription;
        buttonUpgrade.onClick.RemoveAllListeners();
        // Upgrade text message
        String upgradeLabel = "";
        if(upgradeObj.currentLevel >= upgradeObj.numLevels){
            // If the button is maxed out, disable it
            upgradeLabel = "Max";
            buttonUpgrade.GetComponent<Button>().enabled = false;
        }else{
            upgradeLabel = "$" + upgradeObj.GetCostAtLevel(upgradeObj.GetCurrentLevel());
            buttonUpgrade.GetComponent<Button>().enabled = true;
        }
        buttonUpgrade.GetComponentInChildren<TextMeshProUGUI>().text = upgradeLabel;


        buttonUpgrade.GetComponent<Button>().onClick.AddListener(() => ClickUpgrade(upgradeObj, upgradeManager));
        buttonUpgrade.gameObject.SetActive(true);
    }

    public void ClickUpgrade(UpgradeObj upgradeObj, UpgradeManager upgradeManager){
        upgradeManager.UpgradeField(upgradeObj);
        UpdateCurrentUpgradeUI(upgradeObj, upgradeManager);
        RefreshAllUIElements();
    }
}
