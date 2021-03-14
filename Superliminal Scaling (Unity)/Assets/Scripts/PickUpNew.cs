using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpNew : MonoBehaviour
{
    // public float pickupRange = 100f; // TODO: play with this, maybe obsolete
    // test
    public float clampFactor = .01f;
    public float moveForce = 150f;

    private float distanceToHeld; // default value
    private float scaleFactor;   // default value.

    public Transform holdParent;
    private GameObject heldObj;

    public bool collidersVisible = true;
    public Material colliderMaterial;

    private GameObject colliderObj;

    public Transform forwCheck;
    private GameObject forwCollider;

    // public Transform backCheck;
    // private GameObject backCollider;

    public float checkDistance = .5f; // unused

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
            // ScaleObject();
            // Debug.Log(distanceToHeld);

            // Move holdParent along vector input
            if (Input.mouseScrollDelta.y != 0/* && distanceToHeld > 2*/)
            {
                MoveHoldParent(Input.mouseScrollDelta.y);
            }
            ScaleObject();
        }

        // // Move holdParent along vector input
        // if (Input.mouseScrollDelta.y != 0)
        // {
        //     MoveHoldParent(Input.mouseScrollDelta.y);
        // }
    }

    // scales object according to distance from player
    void ScaleObject()
    {
        distanceToHeld = Vector3.Distance(holdParent.transform.position, transform.position);
        float newScale = distanceToHeld * scaleFactor;
        heldObj.transform.localScale = new Vector3(newScale, newScale, newScale);

        // float cS = 1.2f;
        colliderObj.transform.localScale = new Vector3(newScale*1.2f, newScale*1.2f, newScale*1.2f);
    }

    // Moves object to target position until within satisfactory threshold
    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdParent.position) > clampFactor)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
        if (Vector3.Distance(colliderObj.transform.position, holdParent.position) > clampFactor)
        {
            Vector3 moveDirection = (holdParent.position - colliderObj.transform.position);
            colliderObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
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

        // FORWARD COLLIDER
        // move forward-check collider
        // float distanceToForw = Vector3.Distance(transform.position, forwCheck.transform.position);
        // float newScale = distanceToForw * scaleFactor;
        // forwCollider.transform.localScale = new Vector3(newScale, newScale, newScale);
        float cS = 1.1f;
        colliderObj.transform.localScale = new Vector3(newScale * cS, newScale * cS, newScale * cS);
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
            objRig.isKinematic = true;

            // Get distance from Player to Object
            float distanceToObj = Vector3.Distance(transform.position, objRig.transform.position)-2.0f;
            // Debug.Log("Distance:    " + distanceToHeld);
            // Debug.Log("Local Scale: " + pickObj.transform.localScale);
            // Debug.Log(scaleFactor);

            // Move target position (holdParent) to curr. distance along view vector
            holdParent.transform.position += transform.TransformDirection(Vector3.forward) * distanceToObj;

            // Update distance and scaleFactor
            distanceToHeld = Vector3.Distance(holdParent.transform.position, transform.position);
            scaleFactor = pickObj.transform.localScale.x / distanceToHeld;

            // Set object to current held object
            objRig.transform.parent = holdParent;
            heldObj = pickObj;

            // COLLISION
            // GameObject collObj = Instantiate(heldObj, heldObj.transform);
            GameObject collObj = Instantiate(heldObj, holdParent);
            collObj.GetComponent<MeshRenderer>().enabled = collidersVisible;
            collObj.GetComponent<MeshRenderer>().material = colliderMaterial;
            // collObj.AddComponent<Script>() as CollisionChecker;

            Rigidbody collRig = collObj.GetComponent<Rigidbody>();
            collRig.GetComponent<MeshCollider>().isTrigger = true; // ignore physics

            collObj.transform.localScale = new Vector3(2,2,2);
            colliderObj = collObj;

            // -----------------------------------
            // TESTING COLLISION CONSTRAINTS BELOW
            // could be nice if i just never move them 

            // instantiate forward-check collider
            // "public static Object Instantiate(Object original, Transform parent)"
            // GameObject forwObj = Instantiate(heldObj, forwCheck);
            // forwObj.GetComponent<MeshRenderer>().enabled = collidersVisible;
            // forwObj.GetComponent<MeshRenderer>().material = colliderMaterial;

            // Rigidbody forwRig = forwObj.GetComponent<Rigidbody>();
            // forwObj.GetComponent<MeshCollider>().isTrigger = true; // ignore physics

            // forwObj.transform.position = forwCheck.transform.position; // position

            // // scale
            // float forwScale = Vector3.Distance(transform.position, forwCheck.transform.position) * scaleFactor;
            // forwObj.transform.localScale = new Vector3(forwScale,forwScale,forwScale);

            // forwCollider = forwObj;
            // -----------------------------------
        }
    }

    // Reset physics properties, and detach object from holdParent
    void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 1;
        heldRig.freezeRotation = false;
        heldRig.isKinematic = false;
        
        heldObj.transform.parent = null;
        heldObj = null;
        holdParent.transform.position = transform.position + transform.TransformDirection(Vector3.forward)*2f;

        DestroyImmediate(colliderObj);
    }
}
