using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour
{
    private bool isColliding;

    void OnTriggerEnter(Collider other)
    {
        isColliding = true;
    }    

    void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }

    public void IsColliding()
    {
        Debug.Log(isColliding);
    }
}

// !trigger = physics
// trigger = !physics
