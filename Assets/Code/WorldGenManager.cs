using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;


public class WorldGenManager : MonoBehaviour
{


    // Basic world generation with chunks
    // Example: chunkCount = 5
    //          chunkObjectDensity = 2
    //
    //          [Starting Building]
    // 
    //          [ []    Chunk []  ]
    //          [    [] Chunk  [] ]
    //          [[]     Chunk     ]
    //          [   []  Chunk []  ]




    // Prefab to fill chunks
    public GameObject buildingPrefab;

    // Baseplate chunks
    public GameObject baseplatePrefab;

    // Chunk types as follows:
    // Filler - Open spaces for the start
    // Blocks - Main chunks for distance
    // Blocker - Chunks with a blocker at their end
    public GameObject[] fillers;
    public GameObject[] cityBlocks;
    public GameObject[] blockers;



    // Offset Y for generating objects
    public float generationYOffset = 0;

    // # of objects in each chunk
    public float chunkObjectDensity = 1;

    // # of chunks length-wise
    public int chunkCount = 3;
    
    // Width of a given chunk
    public int chunkWidth = 30;
    public int chunkLength = 20;

    public int deadZone = 10;

    void Start()
    {
        SpawnBuildings();
    }

    public void SpawnBuildings(){

        ClearAllChildren();

        for(int i = 0; i < chunkCount; i++){
            GenerateChunk(i);
        }

    }

    private void ClearAllChildren(){
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private GameObject getRandomGameObjectFromArray(GameObject[] arr){
        if (arr == null || arr.Length == 0) {
            return null;
        }

        int randomIndex = Random.Range(0, arr.Length);
        return arr[randomIndex];
    }

    private void GenerateChunk(int chunkNumber){

        GameObject building;
        Vector3 objSpawnLoc;
        objSpawnLoc = transform.position + new Vector3(chunkNumber * chunkLength, 0 + generationYOffset , 0);

        if(chunkNumber == 0){
            building = Instantiate(getRandomGameObjectFromArray(fillers), objSpawnLoc, Quaternion.identity, transform);
            return;
        }else{
            building = Instantiate(getRandomGameObjectFromArray(cityBlocks), objSpawnLoc, Quaternion.identity, transform);
        }

    }
}
