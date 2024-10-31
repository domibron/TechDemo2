using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupableInventoryItem
{
	public GenericItem item { get; set; }


	public void PickUpItem(GenericItem item);
}