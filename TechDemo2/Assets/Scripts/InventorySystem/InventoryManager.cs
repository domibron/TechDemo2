using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class NoSlotAvalibleException : System.Exception
{
	public NoSlotAvalibleException() { }
	public NoSlotAvalibleException(string message) : base(message) { }
	public NoSlotAvalibleException(string message, System.Exception inner) : base(message, inner) { }
	protected NoSlotAvalibleException(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class InventoryManager : MonoBehaviour
{
	public static InventoryManager Instance { get; private set; }

	public Transform PlayerTransform;

	public Dictionary<int, GameObject> ItemsInSlots;

	public GameObject[] Slots;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(Instance);
		}
		else
		{
			Instance = this;
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DropItemFromInventory(int key)
	{
		ItemsInSlots[key].GetComponent<IInventoryItem>().Drop(PlayerTransform.position);

		ItemsInSlots.Remove(key);
	}

	public int AddItemToInventory(GenericItem item)
	{
		try
		{
			int slotToTake = FindNextSlot();


			GameObject newInventoryItem = Instantiate(item.InventoryItemPrefab, Vector3.zero, Quaternion.identity, Slots[slotToTake].transform);


			ItemsInSlots.Add(slotToTake, newInventoryItem);

			return slotToTake;
		}
		catch (NoSlotAvalibleException e)
		{
			throw e;
		}



	}

	private int FindNextSlot()
	{
		int currentSlot = 0;

		foreach (int key in ItemsInSlots.Keys)
		{
			if (key == currentSlot)
			{
				currentSlot++;
			}
		}

		if (currentSlot >= Slots.Length)
		{
			throw new NoSlotAvalibleException("Cannot find a spare slot!");
		}
		else
		{
			return currentSlot;
		}
	}
}
