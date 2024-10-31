using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PickupableInventoryItem
{
	public GenericItem item { get; set; }


	public void PickUpItem(GenericItem item);
}
