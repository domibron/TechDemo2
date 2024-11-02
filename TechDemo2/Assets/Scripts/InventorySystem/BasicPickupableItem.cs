using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class BasicPickupableItem : MonoBehaviour, IPickupableInventoryItem, IInteractable
{
	public GenericItem Item;

	public string ToolTip = "pick up cube.";

	public int InteractableLayer = 6;

	public string InteractableTag = "Interactable";

	GenericItem IPickupableInventoryItem.item => Item;

	string IInteractable.InteractionToolTipDescriptive => ToolTip;


	private Outline outline;


	protected virtual void Start()
	{
		outline = GetComponent<Outline>();

		outline.OutlineWidth = 0;

		gameObject.layer = InteractableLayer;
		gameObject.tag = InteractableTag;
	}





	protected virtual void InteractWithObjectCall()
	{
		(this as IPickupableInventoryItem).PickUpItem();
	}

	void IInteractable.InteractWithObject()
	{
		InteractWithObjectCall();
	}

	protected virtual void OnDeselectedCall()
	{
		outline.OutlineWidth = 0;
	}

	void IInteractable.OnDeselected()
	{
		OnDeselectedCall();
	}

	protected virtual void OnSelectedCall()
	{
		outline.OutlineWidth = 10;
	}


	void IInteractable.OnSelected()
	{
		OnSelectedCall();
	}

	protected virtual void PickUpItemCall()
	{
		try
		{
			InventoryManager.Instance.AddItemToInventory(Item.InventoryItemPrefab, Item.ItemIcon);

			Destroy(this.gameObject);
		}
		catch (NoSlotAvalibleException e)
		{
			print("Cannot pick up item!\n" + e.Message);
		}
	}


	void IPickupableInventoryItem.PickUpItem()
	{
		PickUpItemCall();
	}
}

