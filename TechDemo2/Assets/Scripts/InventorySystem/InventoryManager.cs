using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


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

	public GameObject Inventory;

	[SerializeField]
	public GameObject[] ItemsInSlots;

	public Transform SlotsParent;

	private GameObject[] _slots;

	private bool _IsVisible = false;

	private GameObject _selectedItem = null;

	private int _itemSlot = -1;

	private bool _dragging = false;

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
		List<Transform> children = SlotsParent.GetComponentsInChildren<Transform>().ToList();

		children.RemoveAt(0);

		_slots = new GameObject[children.Count];

		for (int i = 0; i < children.Count; i++)
		{
			_slots[i] = children[i].gameObject;
		}

		ItemsInSlots = new GameObject[_slots.Length];

		_IsVisible = false;
	}

	// Update is called once per frame
	void Update()
	{
		Inventory.SetActive(_IsVisible);


		if (Input.GetKeyDown(KeyCode.I))
		{
			_IsVisible = !_IsVisible;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			List<RaycastResult> results = new List<RaycastResult>();

			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			EventSystem.current.RaycastAll(pointerData, results);



			GameObject potentialSlot = null;

			foreach (var UIItem in results)
			{
				for (int i = 0; i < _slots.Length; i++)
				{
					if (UIItem.gameObject == _slots[i] && ItemsInSlots[i] != null)
					{
						_dragging = true;
						potentialSlot = _slots[i];

						_itemSlot = i;
					}

				}
			}


			_selectedItem = potentialSlot;

			if (_selectedItem == null)
			{
				_itemSlot = -1;
			}


		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			List<RaycastResult> results = new List<RaycastResult>();

			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};
			// ! somthing gberererere asgrh joggbshignsibbaseui gsighisbhguis
			EventSystem.current.RaycastAll(pointerData, results);

			_dragging = false;
		}
	}

	public void DropItemFromInventory(int key)
	{
		ItemsInSlots[key].GetComponent<IInventoryItem>().Drop(PlayerTransform.position);

		ItemsInSlots[key] = null;
	}

	public int AddItemToInventory(GameObject item)
	{
		try
		{
			int slotToTake = FindNextSlot();

			GameObject newInventoryItem = Instantiate(item, Vector3.zero, Quaternion.identity, _slots[slotToTake].transform);

			newInventoryItem.transform.localPosition = Vector3.zero;


			ItemsInSlots[slotToTake] = newInventoryItem;

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

		for (int i = 0; i < ItemsInSlots.Length; i++)
		{
			if (ItemsInSlots[i] == null)
			{
				break;
			}
			else
			{
				currentSlot++;
			}
		}

		if (currentSlot >= _slots.Length)
		{
			throw new NoSlotAvalibleException("Cannot find a spare slot!");
		}
		else
		{
			return currentSlot;
		}
	}
}
