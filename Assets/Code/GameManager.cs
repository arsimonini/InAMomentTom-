using System.Collections;
using System.Collections.Generic;
using OpenCover.Framework.Model;
using UnityEngine;

public enum GameState{
    Launching,
    Launched,
    Upgrading
}

public class GameManager : MonoBehaviour
{

    public LaunchManager launchManager;
    public UpgradeManager upgradeManager;
    public PlayerController playerController;

    // Start is called before the first frame update
    void OnEnable()
    {
        // On start initialize all managers
        bool initManagers = InitializeManagers();
        // Debug log
        if(initManagers){
            Debug.Log("Managers initialized successfully!");
        }
    }

    private bool InitializeManagers(){

        launchManager = GameObject.FindGameObjectsWithTag("LaunchManager")[0].GetComponent<LaunchManager>();
        if(launchManager == null){
            Debug.LogError("Launch Manager failed to load");
            return false;
        }else{
            launchManager.SetGameManager(this);
        }

        upgradeManager = GameObject.FindGameObjectsWithTag("UpgradeManager")[0].GetComponent<UpgradeManager>();
        if(upgradeManager == null){
            Debug.LogError("Upgrade Manager failed to load");
            return false;
        }else{
            upgradeManager.SetGameManager(this);
        }

        playerController = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        if(playerController == null){
            Debug.LogError("Player Controller failed to load!");
            return false;
        }else{
            playerController.SetGameManager(this);
        }

        return true;
    }

    public LaunchManager GetLaunchManager(){
        return launchManager;
    }

    public PlayerController GetPlayerController(){
        return playerController;
    }

    public UpgradeManager GetUpgradeManager(){
        return upgradeManager;
    }

    public GameObject GetPlayerObject(){
        // Assuming the playerController is the object the script is attached to
        return playerController.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            launchManager.RestartLauncherState();
        }
    }
}
