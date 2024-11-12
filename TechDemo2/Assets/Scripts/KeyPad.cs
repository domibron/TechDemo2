using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

	public GameObject RedLight;
	public GameObject GreenLight;

	public Transform CameraTargetLocationAndRotation;

	public TMP_Text Display;

	public string CodeToUnlock = "1234";

	public char[] AllowedInputForCode = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

	public string PlayerTag = "Player";

	public float MaxRange = 15;

	public LayerMask InteractableLayer = 6;

	public string ButtonTag = "Button";

	public float CameraLerpSpeed = 1f;

	public AudioClip GoodBeep;
	public AudioClip BadBeep;
	public AudioClip SingleBeep;

	public UnityEvent OnUnlocked;

	private Transform _playerCameraTransform;
	private Camera _playerCamera;
	private Transform _player;
	private LookController _lookController;
	private MovementController _movementController;
	private InteractableController _interactableController;
	private AudioSource audioSource;

	private bool _playerOnKeypad = false;

	private IInteractable _selectedKey;

	private int _maxDigets = 0;

	private char[] _currentInput;

	private int _currentPlace = 0;

	private float _currentLerpTime = 0;

	private bool _isLerping = false;

	private Quaternion _cameraLastRotation;
	private Vector3 _cameraLastPosition;

	private bool _animationPlaying = false;

	// Start is called before the first frame update
	void Start()
	{
		_player = GameObject.FindWithTag(PlayerTag).transform;
		_lookController = _player.GetComponent<LookController>();
		_movementController = _player.GetComponent<MovementController>();
		_playerCamera = Camera.main;
		_playerCameraTransform = Camera.main.transform;

		audioSource = GetComponent<AudioSource>();

		_interactableController = _player.GetComponent<InteractableController>();

		SetUpKeypad();

		OverlayKeypadForInteractableSystem.SetActive(!_playerOnKeypad);
		RealKeypad.SetActive(_playerOnKeypad);
	}

	// Update is called once per frame
	void Update()
	{
		if (!_playerOnKeypad) return;

		// prevent premature unlocking when using inventory.
		if (_playerOnKeypad && !PauseMenu.Instance.LockEscape)
		{
			PauseMenu.Instance.LockEscape = true;
		}


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

		RedLight.SetActive(true);
		GreenLight.SetActive(false);


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

			if (_animationPlaying)
			{
				StopAllCoroutines();

				ResetKeypadAnimation();
			}

			ClearCurrentInput();

			audioSource.PlayOneShot(SingleBeep);
			UpdateDisplay();

			return;
		}
		else if ((int)buttonType == 11)
		{
			audioSource.PlayOneShot(SingleBeep);
			UpdateDisplay();

			SubmitCode();

			return;
		}
		else if ((int)buttonType == 12)
		{
			if (_animationPlaying)
			{
				StopAllCoroutines();

				ClearCurrentInput();
				ResetKeypadAnimation();
			}

			DeleteLastInCurrentInput();

			audioSource.PlayOneShot(SingleBeep);
			UpdateDisplay();

			return;
		}


		if (_animationPlaying)
		{
			StopAllCoroutines();
			ClearCurrentInput();
			ResetKeypadAnimation();
		}

		EnterNumToKeypad((int)buttonType);

		audioSource.PlayOneShot(SingleBeep);
		UpdateDisplay();

	}

	private void SubmitCode()
	{
		if (_currentInput.ArrayToString() == CodeToUnlock)
		{

			OnUnlocked.Invoke();


			StopAllCoroutines();
			StartCoroutine(CodeFlash(GoodBeep, "green", true));


			AIVoiceManager.Instance.AccessGranted();

			if (_playerOnKeypad) ExitKeypad();
		}
		else
		{

			StopAllCoroutines();
			StartCoroutine(CodeFlash(BadBeep, "red"));


			AIVoiceManager.Instance.AccessDenied();
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
		if (_currentPlace > 0) _currentPlace--;
		_currentInput[_currentPlace] = ' ';
	}

	private void UpdateDisplay()
	{

		Display.text = _currentInput.ArrayToString().Replace(' ', 'X');
	}

	private void UpdateDisplay(string textToDisplay)
	{

		Display.text = textToDisplay;
	}

	private Ray GetRay(Camera camera)
	{
		return camera.ScreenPointToRay(Input.mousePosition);
	}

	public void EnterKeypad()
	{
		if (_playerOnKeypad) return;

		PauseMenu.Instance.LockEscape = true;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

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

		if (!_isLerping || !_playerOnKeypad)
		{
			StartCoroutine(LerpCamera(false));
		}

		PauseMenu.Instance.LockEscape = false;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
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

	private IEnumerator CodeFlash(AudioClip beeps, string colorInRichText, bool isGood = false, bool permGreenLight = false, int durationInSeconds = 2)
	{
		_animationPlaying = true;


		audioSource.clip = beeps;
		audioSource.loop = true;
		audioSource.Play();

		string currentCode = _currentInput.ArrayToString().Replace(' ', 'X');

		ClearCurrentInput();

		if (isGood)
		{
			RedLight.SetActive(false);
			GreenLight.SetActive(true);
		}

		for (int i = 0; i <= durationInSeconds * 2; i++)
		{
			UpdateDisplay($"<color={colorInRichText}>{currentCode}</color>");
			RedLight.SetActive(false);
			GreenLight.SetActive(false);
			yield return new WaitForSeconds(0.25f);

			if (!_animationPlaying)
			{
				ResetKeypadAnimation(permGreenLight);
				break;
			}

			UpdateDisplay($"<color=white>{currentCode}</color>");
			RedLight.SetActive(!isGood);
			GreenLight.SetActive(isGood);
			yield return new WaitForSeconds(0.25f);

			if (!_animationPlaying)
			{
				ResetKeypadAnimation(permGreenLight);
				break;
			}

		}



		if (_animationPlaying)
		{
			ResetKeypadAnimation(permGreenLight);
		}


	}

	private void ResetKeypadAnimation(bool greenLightLit = false)
	{
		RedLight.SetActive(!greenLightLit);
		GreenLight.SetActive(greenLightLit);

		audioSource.Stop();

		UpdateDisplay();

		_animationPlaying = false;
	}
}
