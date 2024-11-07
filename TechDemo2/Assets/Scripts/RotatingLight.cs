using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingLight : MonoBehaviour
{
	public Transform ChildTransform;

	public Vector3 RotationAxis = Vector3.up;

	public float RotationSpeed = 2f;

	private float _rotationAmmount = 360f;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		_rotationAmmount = 360f * RotationSpeed;



		ChildTransform.Rotate(RotationAxis * _rotationAmmount * Time.deltaTime);
	}
}
