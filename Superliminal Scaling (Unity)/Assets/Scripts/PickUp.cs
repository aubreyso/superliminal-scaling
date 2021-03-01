using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // public float pickupRange = 100f; // TODO: play with this, maybe obsolete
    public float clampFactor = .01f;
    public float moveForce = 150f;

    private float distanceToHeld; // default value
    private float scaleFactor;   // default value.
    // private float distanceToHeld = -1; // default value
    // private float scaleFactor = -1f;   // default value.

    public Transform holdParent;
    private GameObject heldObj;

    // Update is called once per frame
    void Update()
    {
        // "E" input (pick-up / drop)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If nothing held,raycast to search for object
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position,                             // ro
                                    transform.TransformDirection(Vector3.forward),  // rd
                                    out hit))                                       // raycast out
                {   
                    PickupObject(hit.transform.gameObject); // layer masks? maybe rigidbody is enough
                }
            }
            // If something held, drop it
            else
            {
                DropObject();
            }
        }
        // Move Object to target position (holdParent)
        if (heldObj != null)
        {
            MoveObject();
    
            // Debug.Log(distanceToHeld);

            // Move holdParent along vector input
            if (Input.mouseScrollDelta.y != 0 && distanceToHeld > 2)
            {
                MoveHoldParent(Input.mouseScrollDelta.y);
            }
        }

        // // Move holdParent along vector input
        // if (Input.mouseScrollDelta.y != 0)
        // {
        //     MoveHoldParent(Input.mouseScrollDelta.y);
        // }
    }

    // Moves object to target position until within satisfactory threshold
    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdParent.position) > clampFactor)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    // Moves holdParent along view vector
    // TODO: ADD FAILSAFE COLLISION STUFF HERE
    void MoveHoldParent(float dir)
    {
        // move object along view vector
        holdParent.transform.position += transform.TransformDirection(Vector3.forward)*dir;
        distanceToHeld = Vector3.Distance(holdParent.transform.position, transform.position);
        // Debug.Log("distance: " + distanceToHeld);
        // Debug.Log("scalefac: " + scaleFactor);

        // GameObject transRef = holdParent.transform.GetChild(0).gameObject;

        float newScale = distanceToHeld * scaleFactor;
        heldObj.transform.localScale = new Vector3(newScale, newScale, newScale);
        // Debug.Log(transRef);
    }

    // Adjust physics properties, and attach object to holdParent
    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        { 
            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;
            objRig.drag = 10;   // optional
            objRig.freezeRotation = true;
            // objRig.isKinematic = true;

            // Move target position (holdParent) to curr. distance along view vector
            float distanceToObj = Vector3.Distance(transform.position, objRig.transform.position)-2.0f;
            // Debug.Log("Distance:    " + distanceToHeld);
            // Debug.Log("Local Scale: " + pickObj.transform.localScale);
            // Debug.Log(scaleFactor);

            holdParent.transform.position += transform.TransformDirection(Vector3.forward) * distanceToObj;

            distanceToHeld = Vector3.Distance(holdParent.transform.position, transform.position);
            scaleFactor = pickObj.transform.localScale.x / distanceToHeld;

            objRig.transform.parent = holdParent;
            heldObj = pickObj;
        }
    }

    // Reset physics properties, and detach object from holdParent
    void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 1;
        heldRig.freezeRotation = false;
        // heldRig.isKinematic = false;
        
        heldObj.transform.parent = null;
        heldObj = null;
        holdParent.transform.position = transform.position + transform.TransformDirection(Vector3.forward)*2f;
    }
}
