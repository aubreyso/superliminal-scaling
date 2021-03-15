using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private bool isColliding;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision enter!");
        isColliding = true;
    }    

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision exit!");
        isColliding = false;
    }

    public bool IsColliding()
    {
        // Debug.Log(isColliding);
        return isColliding;
    }
}

// !trigger = physics
// trigger = !physics
