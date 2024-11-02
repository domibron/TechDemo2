using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicInventoryItem : MonoBehaviour, IInventoryItem
{
	public GenericItem GenericItemSO;

	bool IInventoryItem.IsUsable => GenericItemSO.ItemIsUsable;
	GenericItem IInventoryItem.GenericItemSO => GenericItemSO;


	// Start is called before the first frame update
	protected virtual void Awake()
	{
		GetComponent<Image>().sprite = GenericItemSO.ItemIcon;

		// if (GenericItemSO.ItemIsUsable) throw new NullReferenceException($"Cannot this {nameof(BasicInventoryItem)} for usable items! Please use {nameof(ConsumableInventoryItem)} or any other consumable scritps.");
	}

	protected virtual void DropItemCall(Vector3 targetPosition)
	{
		GameObject go = Instantiate(GenericItemSO.PhysicalItemPrefab, targetPosition, Quaternion.identity);

		Destroy(this.gameObject);
	}

	void IInventoryItem.Drop(Vector3 targetPosition)
	{
		DropItemCall(targetPosition);
	}

	protected virtual void UseItemCall()
	{
		throw new NullReferenceException($"Cannot this {nameof(BasicInventoryItem)} for usable items! Please use {nameof(ConsumableInventoryItem)} or any other consumable scritps.");
	}

	void IInventoryItem.UseItem()
	{
		UseItemCall();
	}
}
