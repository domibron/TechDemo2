using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupableInventoryItem
{
	public GenericItem item { get; }


	public void PickUpItem();
}