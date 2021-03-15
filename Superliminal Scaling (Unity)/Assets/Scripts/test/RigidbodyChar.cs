using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyChar : MonoBehaviour
{
	// Start is called before the first frame update


	public float speed = 12f;
	public float jumpStrength = 5f;

	private Rigidbody _rigidbody;
	private Vector3 _input;
	private Transform _cameraTransform;

	public Transform groundCheck;           // ref to Ground Check obj    
	public float groundDistance = 0.4f;     // radius of Ground Check sphere
	public LayerMask groundMask;            // controls what objects sphere checks for
	private bool _isGrounded;

	void Start()
    {
		_rigidbody = GetComponent<Rigidbody>();
		_input = Vector3.zero;
		_cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
		// _input.x = Input.GetAxis("Horizontal") * _cameraTransform.right;
		//_input.z = Input.GetAxis("Vertical");

		_input = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
		_isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		Debug.Log(_isGrounded);
		if (Input.GetButtonDown("Jump") && _isGrounded)
		{
			_rigidbody.AddForce(Vector3.up * jumpStrength, ForceMode.VelocityChange);
		}
    }

	private void FixedUpdate()
	{
		_rigidbody.MovePosition(_rigidbody.position + _input * speed * Time.fixedDeltaTime);
	}
}
