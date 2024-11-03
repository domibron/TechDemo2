using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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

[Serializable]
public struct DocumentInfo
{
	public string Name;
	public string Info;
	public Sprite Sprite;

	public DocumentInfo(string name, string info, Sprite sprite)
	{
		Name = name;
		Info = info;
		Sprite = sprite;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is DocumentInfo)) return false;


		if (Name == ((DocumentInfo)obj).Name && Info == ((DocumentInfo)obj).Info && Sprite == ((DocumentInfo)obj).Sprite) return false;
		else return true;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public static bool operator ==(DocumentInfo lhs, DocumentInfo rhs)
	{
		return lhs.Equals(rhs);
	}

	public static bool operator !=(DocumentInfo lhs, DocumentInfo rhs)
	{
		return !(lhs == rhs);
	}
}

public class InventoryManager : MonoBehaviour
{
	public static InventoryManager Instance { get; private set; }

	public Transform PlayerTransform;

	public GameObject InventoryTerminal;

	public GameObject InventoryView;
	public GameObject DocumentView;

	[SerializeField]
	public GameObject[] ItemsInSlots;

	public Transform SlotsParent;

	public Button UseItemButton;
	public Button DropItemButton;

	public GameObject CursorImageDragger;
	public Image CursorImageDraggerUIImage;



	[Header("Document stuff")]

	public Transform ScrollContent;
	public GameObject DocumentItemPrefab;


	public TMP_Text DocumentViewerContent;

	public Image DocumentViewerImage;

	public GameObject DocumentViewerGameObject;

	public GameObject TranslateView;

	public TMP_Text TranslateButtonText;

	private bool _translateOpen = false;


	private GameObject[] _slots;

	private bool _IsVisible = false;

	private GameObject _selectedItem = null;

	private int _itemSlot = -1;

	private bool _dragging = false;

	private Dictionary<int, DocumentInfo> _documents = new Dictionary<int, DocumentInfo>();

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

		InventoryTerminal.SetActive(_IsVisible);


		if (Input.GetKeyDown(KeyCode.I))
		{
			_IsVisible = !_IsVisible;
			if (_IsVisible)
			{
				OpenInventory();
			}
		}

		if (_IsVisible && Input.GetKeyDown(KeyCode.Escape))
		{
			_IsVisible = false;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			List<RaycastResult> results = new List<RaycastResult>();

			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			EventSystem.current.RaycastAll(pointerData, results);



			GameObject potentialItem = null;

			foreach (var UIItem in results)
			{
				for (int i = 0; i < _slots.Length; i++)
				{
					if (UIItem.gameObject.transform == _slots[i].transform && ItemsInSlots[i] != null)
					{
						_dragging = true;
						potentialItem = ItemsInSlots[i];

						_itemSlot = i;

						CursorImageDraggerUIImage.sprite = ItemsInSlots[i].GetComponent<IInventoryItem>().GenericItemSO.ItemIcon;
					}

				}
			}

			if (potentialItem != null) _selectedItem = potentialItem;
			else _dragging = false;





		}

		if (_selectedItem != null)
		{
			UseItemButton.interactable = _selectedItem.GetComponent<IInventoryItem>().IsUsable;
			DropItemButton.interactable = true;

		}
		else
		{

			CursorImageDragger.SetActive(false);



			UseItemButton.interactable = false;
			DropItemButton.interactable = false;
		}

		if (_dragging)
		{
			CursorImageDragger.SetActive(true);
			CursorImageDragger.transform.position = Input.mousePosition;
		}
		else
		{
			CursorImageDragger.SetActive(false);

		}

		if (Input.GetKeyUp(KeyCode.Mouse0) && _dragging && _selectedItem != null)
		{
			bool dropItem = true;

			List<RaycastResult> results = new List<RaycastResult>();

			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			EventSystem.current.RaycastAll(pointerData, results);

			foreach (var UIItem in results)
			{
				for (int i = 0; i < _slots.Length; i++)
				{
					if (UIItem.gameObject.transform == _slots[i].transform && ItemsInSlots[i] == null)
					{
						_selectedItem.transform.SetParent(_slots[i].transform);

						_selectedItem.transform.localPosition = Vector3.zero;

						ItemsInSlots[i] = _selectedItem;

						ItemsInSlots[_itemSlot] = null;

						_itemSlot = i;

						dropItem = false;

						// _dragging = false;
					}
					else if (UIItem.gameObject == _slots[i] && ItemsInSlots[i] != null)
					{
						dropItem = false;

						// _dragging = false;
					}

				}
			}

			if (dropItem)
			{
				DropItemFromInventory(_itemSlot);
			}

			_dragging = false;

			// _selectedItem = null;
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			ToggleTranslate();
		}
	}

	public void UseItem()
	{
		if (_selectedItem == null || _itemSlot == -1) return;

		_selectedItem.GetComponent<IInventoryItem>().UseItem();
	}

	public void DropItem()
	{
		if (_selectedItem == null || _itemSlot == -1) return;

		DropItemFromInventory(_itemSlot);
	}

	public void DropItemFromInventory(int itemInSlot)
	{
		_itemSlot = -1;

		_selectedItem = null;

		ItemsInSlots[itemInSlot].GetComponent<IInventoryItem>().Drop(PlayerTransform.position);

		ItemsInSlots[itemInSlot] = null;
	}

	public int AddItemToInventory(GameObject item, Sprite icon)
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

	public int AddDocumentToInventory(DocumentInfo documentInfo)
	{
		foreach (var item in _documents)
		{
			if (item.Value == documentInfo)
			{
				return item.Key;
			}
		}

		int place = _documents.Count;
		_documents.Add(place, documentInfo);

		GameObject go = Instantiate(DocumentItemPrefab, ScrollContent);

		go.GetComponent<DocumentViewItem>().SetUpItem(documentInfo, place);

		// go.transform.position = Vector3.zero;



		return place;
	}

	public void OpenDocumentView()
	{
		_IsVisible = true;

		DocumentView.SetActive(true);
		InventoryView.SetActive(false);
		_translateOpen = false;
		TranslateButtonText.text = "Translate (T)";
		TranslateView.SetActive(false);
		DocumentViewerGameObject.SetActive(false);
	}

	public void OpenInventory()
	{

		DocumentView.SetActive(false);
		InventoryView.SetActive(true);
	}

	public void OpenDoc(int id)
	{
		OpenDocumentView();
		DocumentViewerGameObject.SetActive(true);
		DocumentViewerContent.text = _documents[id].Info;
		DocumentViewerImage.sprite = _documents[id].Sprite;
	}

	public void CloseDocument()
	{
		DocumentViewerGameObject.SetActive(false);
	}

	public void ToggleTranslate()
	{
		_translateOpen = !_translateOpen;

		if (_translateOpen)
		{
			TranslateView.SetActive(true);
			TranslateButtonText.text = "Stop Translate (T)";
		}
		else
		{
			TranslateView.SetActive(false);
			TranslateButtonText.text = "Translate (T)";
		}

	}
}
