using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // Get a reference to the RB on the object
    private Rigidbody rb;

    // Public spped modifier variable
    public float moveSpeed = 1;
    void Start()
    {
        if(!TryGetComponent<Rigidbody>(out rb)){
            Debug.LogError("PlayerController can't find a RigidBody!");
        }
    }

    void FixedUpdate()
    {
        // Safety check conf. RB exist
        if(rb != null){
            ApplyMove();
        }
    }

    private void ApplyMove(){
        // Basic axis input movement
        float axisInput = Input.GetAxis("Horizontal");
        Vector3 moveVector = transform.right * axisInput;
        rb.MovePosition(transform.position + moveVector * Time.fixedDeltaTime * moveSpeed);   
    }
}
