using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericItem", menuName = "PickupableItems/GenericItem", order = 0)]
public class GenericItem : ScriptableObject
{
	public string Name = "Item";
	public string Description = "A item.";
	public Sprite ItemIcon;
	public bool ItemIsUsable = false;
	public GameObject PhysicalItemPrefab;
	public GameObject InventoryItemPrefab;
}
