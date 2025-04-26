using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeObj : ScriptableObject
{
    public Sprite icon;
    public string upgradeName;
    public string upgradeID;
    public string upgradeDescription;
    public int currentLevel;
    public int numLevels;
    public int[] levelCosts;

    public int GetCurrentLevel(){
        return currentLevel;
    }

    public int GetCostAtLevel(int level){
        if(level >= 0 && level < numLevels){
            return levelCosts[level];
        }
        else{
            return -1;
        }
    }

    public void Upgrade(){
        if(currentLevel < numLevels){
            currentLevel++;
        }
    }
}
