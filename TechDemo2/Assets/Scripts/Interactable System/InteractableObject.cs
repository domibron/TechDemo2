using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
	public string ToolTip = "interact.";

	string IInteractable.InteractionToolTipDescriptive { get => ToolTip; }



	void IInteractable.InteractWithObject()
	{
		throw new System.NotImplementedException();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
