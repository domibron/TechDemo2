using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Items/GenericItem")]
public class GenericItem
{
	public string Name = "Item";
	public string Description = "A item.";
	public Sprite ItemIcon;
	public bool ItemIsUsable = false;
	public GameObject PhysicalItemPrefab;
	public GameObject InventoryItemPrefab;
}
