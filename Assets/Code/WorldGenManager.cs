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

    private void GenerateChunk(int chunkNumber){

        for(int i = 0; i < chunkObjectDensity; i++){
            Vector3 objSpawnLoc = transform.position + new Vector3(chunkNumber * chunkLength, 0 , Random.Range(-chunkWidth/2,chunkWidth/2));
            GameObject building = Instantiate(buildingPrefab, objSpawnLoc, Quaternion.identity, transform);
        }

    }
}
