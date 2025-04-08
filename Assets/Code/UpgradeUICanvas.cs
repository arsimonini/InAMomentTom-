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

    public Button buttonUpgrade;

    public Transform upgradeButtonContainerPanel;

    public GameObject upgradeButtonPrefab;

    public void setMoneyText(float newMoney){
        moneyText.text = ("$" + newMoney.ToString("0"));
    }

    void Start()
    {
        buttonUpgrade.gameObject.SetActive(false);
    }

    public void CreateUpgradeButton(UpgradeObj upgrade, UpgradeManager upgradeManager){
        // Create a new empty upgrade button to populate
        GameObject newButton = Instantiate(upgradeButtonPrefab, upgradeButtonContainerPanel);
        // Populate base icon and onclick event
        newButton.GetComponent<Image>().sprite = upgrade.icon;
        newButton.GetComponent<Button>().onClick.AddListener(() => UpdateCurrentUpgradeUI(upgrade, upgradeManager));
    }

    public void UpdateCurrentUpgradeUI(UpgradeObj upgradeObj, UpgradeManager upgradeManager){
        // Update the info pane with relevant info on click
        cUpgradeSprite.sprite = upgradeObj.icon;
        cUpgradeTitle.text = upgradeObj.upgradeName;
        cUpgradeDescription.text = upgradeObj.upgradeDescription;
        buttonUpgrade.GetComponent<Button>().onClick.AddListener(() => upgradeManager.UpgradeField(upgradeObj));
        buttonUpgrade.gameObject.SetActive(true);
    }
}
