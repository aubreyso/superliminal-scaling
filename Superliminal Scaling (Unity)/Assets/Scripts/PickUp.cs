using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // public float pickupRange = 100f; // TODO: play with this, maybe obsolete
    public float clampFactor = .01f;
    public float moveForce = 150f;

    public Transform holdParent;
    private GameObject heldObj;

    // Update is called once per frame
    void Update()
    {
        // Pickup / Drop input
        if (Input.GetKeyDown(KeyCode.E)) // TODO: replace with mouse input?
        {
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
            else
            {
                DropObject();
            }
        }
        if (heldObj != null)
        {
            MoveObject();
        }

        // Push along vector input
        if (Input.mouseScrollDelta.y != 0)
        {
            // Debug.Log(Input.mouseScrollDelta.y);
            MoveHoldParent(Input.mouseScrollDelta.y);
        }
        // Debug.Log(Input.mouseScrollDelta);
    }

    void MoveObject()
    {
        // move object to view
        if (Vector3.Distance(heldObj.transform.position, holdParent.position) > clampFactor)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void MoveHoldParent(float dir)
    {
        // Debug.Log(holdParent.transform.position.z);
        // if (holdParent.transform.position.z > .2)
        holdParent.transform.position += transform.TransformDirection(Vector3.forward)*dir;

        GameObject transRef = holdParent.transform.GetChild(0).GetChild(0).gameObject;
        Debug.Log(transRef);
        transRef.transform.localScale += new Vector3(dir,dir,dir) * 0.2f;
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            // holdParent.transform.position = 
            Rigidbody objRig = pickObj.GetComponent<Rigidbody>();
            objRig.useGravity = false;
            objRig.drag = 10;   // optional
            objRig.freezeRotation = true;
            // objRig.isKinematic = true;

            holdParent.transform.position = objRig.transform.position;
            objRig.transform.parent = holdParent;
            heldObj = pickObj;
        }
    }

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
