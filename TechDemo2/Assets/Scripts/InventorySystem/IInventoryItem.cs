using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
	public bool IsUsable { get; }

	public GenericItem GenericItemSO { get; set; }

	public void Drop(Vector3 targetPosition);

	public void UseItem();
}
