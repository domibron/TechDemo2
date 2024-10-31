using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickupableItem : MonoBehaviour, IPickupableInventoryItem, IInteractable
{
	GenericItem IPickupableInventoryItem.item { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	string IInteractable.InteractionToolTipDescriptive => throw new System.NotImplementedException();




	void IInteractable.InteractWithObject()
	{
		throw new System.NotImplementedException();
	}

	void IInteractable.OnDeselected()
	{
		throw new System.NotImplementedException();
	}

	void IInteractable.OnSelected()
	{
		throw new System.NotImplementedException();
	}

	void IPickupableInventoryItem.PickUpItem(GenericItem item)
	{
		throw new System.NotImplementedException();
	}
}
