using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Document/DocumentSO")]
public class DocumentSO : ScriptableObject
{
	public string Name;

	[TextArea(5, 500)]
	public string Content;

	public Sprite DocumentSprite;
}