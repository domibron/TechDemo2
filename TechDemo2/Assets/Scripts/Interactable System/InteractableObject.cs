using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class InteractableObject : MonoBehaviour, IInteractable
{
	[Header("Interactable Defualts")]
	public int InteractableLayer = 6;
	public string InteractableTag = "Interactable";

	[Header("Interactable Settings")]
	public float OutlineWidth = 10f;
	public string ToolTip = "interact.";

	[Header("Event")]
	public UnityEvent OnInteractEvent;

	private Outline _outline;

	string IInteractable.InteractionToolTipDescriptive { get => ToolTip; }



	void IInteractable.InteractWithObject()
	{
		OnInteractEvent.Invoke();
	}

	void IInteractable.OnSelected()
	{
		_outline.OutlineWidth = OutlineWidth;

	}

	void IInteractable.OnDeselected()
	{
		_outline.OutlineWidth = 0f;

	}

	private void Awake()
	{
		gameObject.layer = InteractableLayer;
		gameObject.tag = InteractableTag;


	}

	private void Start()
	{
		_outline = GetComponent<Outline>();

		_outline.OutlineWidth = 0f;
	}
}
