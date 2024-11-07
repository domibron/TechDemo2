using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyPadButtonType
{
	Zero = 0,
	One = 1,
	Two = 2,
	Three = 3,
	Four = 4,
	Five = 5,
	Six = 6,
	Seven = 7,
	Eight = 8,
	Nine = 9,
	Clear = 10,
	Submit = 11,
	Delete = 12
}

[RequireComponent(typeof(Outline))]
public class KeyPadButton : MonoBehaviour, IInteractable
{
	public KeyPadButtonType KeyPadButtonType;


	public int InteractableLayer = 6;

	public string ButtonTag = "Button";

	public float OutlineWidth = 10f;

	string IInteractable.InteractionToolTipDescriptive => $"enter {KeyPadButtonType}";


	private KeyPad _keyPadParent;

	private Outline _outline;

	private Animator _buttonAnimator;


	void Start()
	{
		gameObject.tag = ButtonTag;
		gameObject.layer = InteractableLayer;

		_outline = GetComponent<Outline>();
		_buttonAnimator = GetComponent<Animator>();

		_keyPadParent = GetComponentInParent<KeyPad>();

		_outline.OutlineWidth = 0;

	}

	void IInteractable.InteractWithObject()
	{
		_buttonAnimator.SetTrigger("ButtonPressed");
		_keyPadParent.EnterKey(KeyPadButtonType);
	}

	void IInteractable.OnDeselected()
	{
		_outline.OutlineWidth = 0;
	}

	void IInteractable.OnSelected()
	{
		_outline.OutlineWidth = OutlineWidth;
	}
}
