using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyLook : MonoBehaviour
{
	// Start is called before the first frame update
	public float mouseSensitivity = 400f;
	float xRotation = 0f;

	private Rigidbody _rigidbody;
    void Start()
    {
		_rigidbody = gameObject.GetComponentInParent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}

    // Update is called once per frame
    void Update()
    {
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		// clamp rotation
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		//_rigidbody.MoveRotation(Vector3.up * mouseX);
	}
}
