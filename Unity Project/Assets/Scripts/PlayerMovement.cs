using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;   // reference to Character Controller component

    // general movement constants
    public float speed = 12f;         // move speed
    public float gravity = -9.81f;   // gravity 
    public float jumpHeight = 3;

    // ground check logic variables
    public Transform groundCheck;           // ref to Ground Check obj    
    public float groundDistance = 0.4f;     // radius of Ground Check sphere
    public LayerMask groundMask;            // controls what objects sphere checks for

    // current status variables
    Vector3 velocity;               // multiplies with gravity for non-linear fall 
    bool isGrounded;                // bool: is player grounded?

    // Update is called once per frame
    void Update()
    {
        // Create a sphere @ position w groundDistance radius, check for collisions with groundMask
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Move input
        float xin = Input.GetAxis("Horizontal");
        float zin = Input.GetAxis("Vertical");

        // Vector (arrow) pointing in move dir.
        Vector3 moveVec = (transform.right * xin) + (transform.forward * zin);
        controller.Move(moveVec * speed * Time.deltaTime);

        // Apply jump
        // ...

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
