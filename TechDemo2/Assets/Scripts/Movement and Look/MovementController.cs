using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
	public bool Locked = false;

	public float Speed = 15;

	public float GroundCheckRadius = 0.8f;

	public LayerMask GroundCheckIgnoredLayers;

	public float Gravity = -9.81f;

	public float JumpHeight = 2f;


	// replace with somthing else.
	public float PlayerHeight = 2f;


	private CharacterController _characterController;

	private Vector3 _velocity;

	private bool _isGrounded;

	// Start is called before the first frame update
	void Start()
	{
		_characterController = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Locked) return;

		HandleGroundCheck();

		HandleGravity();

		HandleJumping();

		HandleVelocityMovement();

		HandleMovement();
	}

	private void HandleMovement()
	{
		Vector3 MoveDirection = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");


		_characterController.Move(MoveDirection * Speed * Time.deltaTime);
	}

	private void HandleGroundCheck()
	{
		if (Physics.CheckSphere(transform.position - transform.up * (PlayerHeight / 2f), GroundCheckRadius, ~GroundCheckIgnoredLayers))
		{
			_isGrounded = true;
		}
		else
		{
			_isGrounded = false;
		}
	}

	private void HandleGravity()
	{
		if (_isGrounded && _velocity.y < Gravity)
		{
			_velocity.y = Gravity;
		}
		else
		{
			_velocity.y += Gravity * Time.deltaTime;
		}


	}

	private void HandleVelocityMovement()
	{
		_characterController.Move(_velocity * Time.deltaTime);
	}

	private void HandleJumping()
	{
		if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
		{


			_velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity) - _velocity.y;
		}
	}
}
