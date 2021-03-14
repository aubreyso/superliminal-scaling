using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    void OnCollisionEnter()
    {
        Debug.Log("enter!");
    }
    void OnCollisionExit()
    {
        Debug.Log("exit!");
    }
}
