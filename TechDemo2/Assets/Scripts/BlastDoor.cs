using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastDoor : MonoBehaviour
{
	private Animator animator;

	public bool DoorIsOpen = false;

	public bool DoorIsLocked = false;

	// Start is called before the first frame update
	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		animator.SetBool("DoorOpen", DoorIsOpen);
	}

	public void OpenDoor()
	{
		if (DoorIsLocked) return;

		DoorIsOpen = true;
	}

	public void CloseDoor()
	{
		if (DoorIsLocked) return;

		DoorIsOpen = false;
	}

	public void SetDoorIsOpenState(bool state)
	{
		if (DoorIsLocked) return;

		DoorIsOpen = state;
	}

	public void ToggleDoorOpen()
	{
		if (DoorIsLocked) return;

		DoorIsOpen = !DoorIsOpen;
	}

	public void LockDoor()
	{
		DoorIsLocked = true;
	}

	public void UnlockDoor()
	{
		DoorIsLocked = false;
	}

	public void SetDoorLockState(bool state)
	{
		DoorIsLocked = state;
	}

	public void OverrideDoorOpen()
	{
		DoorIsOpen = true;
	}

	public void OverrideDoorClose()
	{
		DoorIsOpen = true;
	}

	public void OverrideSetDoorOpenState(bool state)
	{
		DoorIsOpen = state;
	}

	public void OverrideToggleDoorOpen()
	{
		DoorIsOpen = !DoorIsOpen;
	}
}
