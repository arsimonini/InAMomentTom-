using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUICanvas : MonoBehaviour
{

    public TMP_Text moneyText;

    public Image cUpgradeSprite;
    public TMP_Text cUpgradeTitle;
    public TMP_Text cUpgradeDescription;

    public Transform upgradeButtonContainerPanel;

    public GameObject upgradeButtonPrefab;

    public void setMoneyText(float newMoney){
        moneyText.text = ("$" + newMoney.ToString("0"));
    }

    public void CreateUpgradeButton(UpgradeObj upgrade){
        // Create a new empty upgrade button to populate
        GameObject newButton = Instantiate(upgradeButtonPrefab, upgradeButtonContainerPanel);
        // Populate base icon and onclick event
        newButton.GetComponent<Image>().sprite = upgrade.icon;
        newButton.GetComponent<Button>().onClick.AddListener(() => UpdateCurrentUpgradeUI(upgrade));
    }

    public void UpdateCurrentUpgradeUI(UpgradeObj upgradeObj){
        // Update the info pane with relevant info on click
        cUpgradeSprite.sprite = upgradeObj.icon;
        cUpgradeTitle.text = upgradeObj.upgradeName;
        cUpgradeDescription.text = upgradeObj.upgradeDescription;
    }
}
