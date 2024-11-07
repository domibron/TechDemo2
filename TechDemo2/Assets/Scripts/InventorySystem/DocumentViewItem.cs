using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DocumentViewItem : MonoBehaviour
{
	public DocumentInfo DocumentInfoForThis;

	public TMP_Text DisplayText;

	private int ID;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SelectThisDocument()
	{
		InventoryManager.Instance.OpenDoc(ID);
	}

	public void SetUpItem(DocumentInfo documentInfo, int id)
	{
		DocumentInfoForThis = documentInfo;

		DisplayText.text = documentInfo.Name;

		ID = id;
	}
}
