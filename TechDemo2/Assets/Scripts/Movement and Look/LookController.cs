using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookController : MonoBehaviour
{
	public bool Locked = false;

	public float Sensitivity = 1f;

	public Transform CameraHolder;

	public float MaxViewingAngle = 80;

	float _xRotation;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Locked) return;

		HandleLook();
	}

	private void HandleLook()
	{
		float mouseInputX = Input.GetAxisRaw("Mouse X");
		float mouseInputY = Input.GetAxisRaw("Mouse Y");


		_xRotation -= mouseInputY * Sensitivity;

		Mathf.Clamp(_xRotation, -MaxViewingAngle, MaxViewingAngle);


		CameraHolder.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

		transform.Rotate(transform.up * mouseInputX * Sensitivity);
	}
}
