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
    public PlayerUICanvas playerUICanvas;
    public WorldGenManager worldGenManager;

    public GameState currentGameState;

    // Awake is like Start but it goes before it
    void Awake()
    {
        // On awake initialize all managers
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

        playerUICanvas = GameObject.FindGameObjectsWithTag("UI_Canvas")[0].GetComponent <PlayerUICanvas>();
        if(playerUICanvas == null){
            Debug.LogError("Player UI failed to load!");
            return false;
        }

        worldGenManager = GameObject.FindGameObjectsWithTag("WorldGeneratorManager")[0].GetComponent<WorldGenManager>();
        if(worldGenManager == null){
            Debug.LogError("WorldGenManager failed to load!");
            return false;
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

    public PlayerUICanvas GetPlayerUICanvas(){
        return playerUICanvas;
    }

    public GameState GetGameState(){
        return currentGameState;
    }
    public void SetGameState(GameState gameState){
        currentGameState = gameState;
    }

    public GameObject GetPlayerObject(){
        // Assuming the playerController is the object the script is attached to
        return playerController.gameObject;
    }

    public void ResetPlayer(){
        if(currentGameState == GameState.Launched){
            // Should go to upgrades, TODO: Set gamestate to upgrading instead
            SetGameState(GameState.Launching);

            launchManager.RestartLauncherState();
            playerController.ResetPlayer();
            // Every time the player resets, spawn a new world
            worldGenManager.SpawnBuildings();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            ResetPlayer();
        }
    }
}
