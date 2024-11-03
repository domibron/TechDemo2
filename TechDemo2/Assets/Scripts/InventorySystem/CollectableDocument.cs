using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CollectableDocument : MonoBehaviour, IInteractable
{
	private string _toolTip = "pickup the document";

	string IInteractable.InteractionToolTipDescriptive => _toolTip;

	public DocumentSO document;

	private bool _alreadyCollected = false;

	private int _interactableLayer = 6;

	private string _interactableTag = "Interactable";

	private int _id = -1;

	private Outline outline;

	void Awake()
	{
		gameObject.tag = _interactableTag;
		gameObject.layer = _interactableLayer;

		outline = GetComponent<Outline>();

		outline.OutlineWidth = 0;
	}

	void IInteractable.InteractWithObject()
	{
		CollectDocument();
	}

	void IInteractable.OnDeselected()
	{
		outline.OutlineWidth = 0f;
	}

	void IInteractable.OnSelected()
	{
		outline.OutlineWidth = 10f;
	}

	private void CollectDocument()
	{
		if (!_alreadyCollected)
		{
			_id = InventoryManager.Instance.AddDocumentToInventory(new DocumentInfo(document.name, document.Content, document.DocumentSprite));
			_alreadyCollected = true;
		}


		InventoryManager.Instance.OpenDoc(_id);



	}
}
