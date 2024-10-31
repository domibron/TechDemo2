using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPickupableItem : MonoBehaviour, IPickupableInventoryItem, IInteractable
{
	public GenericItem Item;

	public string ToolTip = "pick up cube.";

	public int InteractableLayer = 6;

	public string InteractableTag = "Interactable";

	GenericItem IPickupableInventoryItem.item => Item;

	string IInteractable.InteractionToolTipDescriptive => ToolTip;


	private Outline outline;


	void Start()
	{
		outline = GetComponent<Outline>();

		outline.OutlineWidth = 0;

		gameObject.layer = InteractableLayer;
		gameObject.tag = InteractableTag;
	}





	void IInteractable.InteractWithObject()
	{
		(this as IPickupableInventoryItem).PickUpItem();
	}

	void IInteractable.OnDeselected()
	{
		outline.OutlineWidth = 0;
	}

	void IInteractable.OnSelected()
	{
		outline.OutlineWidth = 10;
	}

	void IPickupableInventoryItem.PickUpItem()
	{
		try
		{
			InventoryManager.Instance.AddItemToInventory(Item.InventoryItemPrefab);

			Destroy(this.gameObject);
		}
		catch (NoSlotAvalibleException e)
		{
			print("Cannot pick up item!\n" + e.Message);
		}
	}
}
