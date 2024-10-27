using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InvalidValueExeption : System.Exception
{
	public InvalidValueExeption() { }
	public InvalidValueExeption(string message) : base(message) { }
	public InvalidValueExeption(string message, System.Exception inner) : base(message, inner) { }
	protected InvalidValueExeption(
		System.Runtime.Serialization.SerializationInfo info,
		System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public class KeyPad : MonoBehaviour
{
	public GameObject RealKeypad;
	public GameObject OverlayKeypadForInteractableSystem;

	public Transform CameraTargetLocationAndRotation;

	public TMP_Text Display;

	public string CodeToUnlock = "1234";

	public char[] AllowedInputForCode = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

	public string PlayerTag = "Player";

	public float MaxRange = 15;

	public LayerMask InteractableLayer = 6;

	public string ButtonTag = "Button";

	public float CameraLerpSpeed = 1f;

	public UnityEvent OnUnlocked;

	private Transform _playerCameraTransform;
	private Camera _playerCamera;
	private Transform _player;
	private LookController _lookController;
	private MovementController _movementController;
	private InteractableController _interactableController;

	private bool _playerOnKeypad = false;

	private IInteractable _selectedKey;

	private int _maxDigets = 0;

	private char[] _currentInput;

	private int _currentPlace = 0;

	private float _currentLerpTime = 0;

	private bool _isLerping = false;

	private Quaternion _cameraLastRotation;
	private Vector3 _cameraLastPosition;

	// Start is called before the first frame update
	void Start()
	{
		_player = GameObject.FindWithTag(PlayerTag).transform;
		_lookController = _player.GetComponent<LookController>();
		_movementController = _player.GetComponent<MovementController>();
		_playerCamera = Camera.main;
		_playerCameraTransform = Camera.main.transform;

		_interactableController = _player.GetComponent<InteractableController>();

		SetUpKeypad();

		OverlayKeypadForInteractableSystem.SetActive(!_playerOnKeypad);
		RealKeypad.SetActive(_playerOnKeypad);
	}

	// Update is called once per frame
	void Update()
	{
		if (!_playerOnKeypad) return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ExitKeypad();
		}

		if (Physics.Raycast(GetRay(_playerCamera), out RaycastHit hit, MaxRange, InteractableLayer))
		{
			if (hit.transform.CompareTag(ButtonTag)) SelectKey(hit.transform.GetComponent<IInteractable>());
			else DeselectKey();
		}
		else
		{
			DeselectKey();
		}

		if (_selectedKey != null && Input.GetKeyDown(KeyCode.Mouse0))
		{
			_selectedKey.InteractWithObject();
		}
	}

	private void SetUpKeypad()
	{
		if (CodeToUnlock.Any(x => !AllowedInputForCode.Contains(x)))
		{
			throw new InvalidValueExeption($"{CodeToUnlock} contains a forbidden value, either add it to the {nameof(AllowedInputForCode)} or fix {nameof(CodeToUnlock)}'s value!");
		}

		_maxDigets = CodeToUnlock.ToCharArray().Length;

		_currentInput = new char[_maxDigets];

		for (int i = 0; i < _currentInput.Length; i++)
		{
			_currentInput[i] = ' ';
		}

		_currentPlace = 0;

		UpdateDisplay();
	}

	private void SelectKey(IInteractable keyToSelect)
	{
		if (keyToSelect == null) return;

		DeselectKey();

		_selectedKey = keyToSelect;

		keyToSelect.OnSelected();
	}

	private void DeselectKey()
	{
		if (_selectedKey != null) _selectedKey.OnDeselected();

		_selectedKey = null;
	}

	public void EnterKey(KeyPadButtonType buttonType)
	{
		if ((int)buttonType == 10)
		{
			ClearCurrentInput();
			UpdateDisplay();
			return;
		}
		else if ((int)buttonType == 11)
		{
			SubmitCode();
			UpdateDisplay();
			return;
		}
		else if ((int)buttonType == 12)
		{
			DeleteLastInCurrentInput();
			UpdateDisplay();
			return;
		}

		EnterNumToKeypad((int)buttonType);
		UpdateDisplay();
		// any other keys
	}

	private void SubmitCode()
	{
		if (_currentInput.ArrayToString() == CodeToUnlock)
		{
			print("YIPPIE");
			OnUnlocked.Invoke();
		}
	}

	private void EnterNumToKeypad(int key)
	{
		if (_currentPlace < _maxDigets)
		{
			_currentInput[_currentPlace] = char.Parse(key.ToString());
			_currentPlace++;
		}
	}

	private void ClearCurrentInput()
	{
		for (int i = 0; i < _currentInput.Length; i++)
		{
			_currentInput[i] = ' ';
		}
		_currentPlace = 0;
	}

	private void DeleteLastInCurrentInput()
	{
		_currentPlace--;
		_currentInput[_currentPlace] = ' ';
	}

	private void UpdateDisplay()
	{

		Display.text = _currentInput.ArrayToString().Replace(' ', 'X');
	}

	private Ray GetRay(Camera camera)
	{
		return camera.ScreenPointToRay(Input.mousePosition);
	}

	public void EnterKeypad()
	{
		_playerOnKeypad = true;

		_interactableController.Locked = _playerOnKeypad;
		_lookController.Locked = _playerOnKeypad;
		_movementController.Locked = _playerOnKeypad;

		OverlayKeypadForInteractableSystem.SetActive(!_playerOnKeypad);
		RealKeypad.SetActive(_playerOnKeypad);

		if (!_isLerping)
		{
			_cameraLastRotation = _playerCameraTransform.rotation;
			_cameraLastPosition = _playerCameraTransform.position;

			StartCoroutine(LerpCamera());
		}
	}

	public void ExitKeypad()
	{

		if (!_isLerping)
		{
			StartCoroutine(LerpCamera(false));
		}
	}

	private void SetKeypadState(bool state)
	{
		_playerOnKeypad = state;

		_interactableController.Locked = _playerOnKeypad;
		_lookController.Locked = _playerOnKeypad;
		_movementController.Locked = _playerOnKeypad;

		OverlayKeypadForInteractableSystem.SetActive(!_playerOnKeypad);
		RealKeypad.SetActive(_playerOnKeypad);
	}

	private IEnumerator LerpCamera(bool lerpTowardsTarget = true)
	{
		if (lerpTowardsTarget)
		{
			_currentLerpTime = 0f;
		}
		else
		{
			_currentLerpTime = 1f;
		}

		_isLerping = true;


		while ((lerpTowardsTarget && _currentLerpTime <= 1) || (!lerpTowardsTarget && _currentLerpTime >= 0))
		{
			yield return null;

			if (lerpTowardsTarget && _currentLerpTime <= 1)
			{
				_currentLerpTime += Time.deltaTime * CameraLerpSpeed;
			}
			else if (!lerpTowardsTarget && _currentLerpTime >= 0)
			{
				_currentLerpTime -= Time.deltaTime * CameraLerpSpeed;
			}

			_playerCameraTransform.rotation = Quaternion.Lerp(_cameraLastRotation, CameraTargetLocationAndRotation.rotation, _currentLerpTime);
			_playerCameraTransform.position = Vector3.Lerp(_cameraLastPosition, CameraTargetLocationAndRotation.position, _currentLerpTime);

		}





		_isLerping = false;

		// uhhhh. should be fine.
		SetKeypadState(lerpTowardsTarget);


	}
}
