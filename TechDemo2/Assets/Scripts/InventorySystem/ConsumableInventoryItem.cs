using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsumableInventoryItem : BasicInventoryItem
{
	protected virtual void UseConsumableItem()
	{
		Destroy(this.gameObject);
	}

	protected override void UseItemCall()
	{
		UseConsumableItem();
	}
}
