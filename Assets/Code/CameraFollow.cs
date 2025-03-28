using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    // this should autofill at runtime
    private GameObject player;
    
    // Default offset
    private Vector3 offset;

    // How fast the camera smooths to the player
    public float smoothSpeed = 0.125f;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(player != null){
            offset = transform.position - player.transform.position;
        }else{
            // Use a default offset if the player doesn't exist, shouldn't ever happen
            offset = new Vector3(0f, 5f, -10f);
        }
    }

    void FixedUpdate()
    {
        if(player != null){
            Vector3 targetPos = player.transform.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);

            // Update position to match with the smoothed pos
            transform.position = smoothedPos;
        }
    }
}
