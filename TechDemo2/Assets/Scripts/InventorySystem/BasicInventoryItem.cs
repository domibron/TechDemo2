using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInventoryItem : MonoBehaviour, IInventoryItem
{
	public GenericItem GenericItemSO;

	bool IInventoryItem.IsUsable { get => GenericItemSO.ItemIsUsable; }
	GenericItem IInventoryItem.GenericItemSO { get => GenericItemSO; set => GenericItemSO = value; }

	private int key;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void IInventoryItem.Drop(Vector3 targetPosition)
	{
		GameObject go = Instantiate(GenericItemSO.PhysicalItemPrefab, targetPosition, Quaternion.identity);


	}

	void IInventoryItem.UseItem()
	{

		throw new System.NotImplementedException();
	}
}
