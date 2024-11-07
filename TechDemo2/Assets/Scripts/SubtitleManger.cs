using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleManger : MonoBehaviour
{
	public static SubtitleManger Instance;

	public TMP_Text SubtitleTextbox;

	public char StartKey = '{';
	public char EndKey = '}';

	public char PauseTimingKey = 'p';
	public char CharacterTimingKey = 'c';
	public char ClearKey = 'e';

	private bool _alreadyDisplayingText = false;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}

		SubtitleTextbox.gameObject.SetActive(false);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void DisplayText(string textToDisplay)
	{
		if (_alreadyDisplayingText)
		{
			_alreadyDisplayingText = false;
			StopAllCoroutines();
		}

		StartCoroutine(AnimateText(textToDisplay));
	}

	private IEnumerator AnimateText(string textToDisplay)
	{
		_alreadyDisplayingText = true;

		string currentText = "";

		char[] charactersInText = textToDisplay.ToCharArray();

		bool isSpecialKey = false;

		float textSpeed = 0.1f;

		bool isCharacterSpeed = false;
		bool isPauseDuration = false;

		bool isClearKey = false;

		string timeDurationAsText = "";

		bool exitNoReset = false;

		SubtitleTextbox.gameObject.SetActive(true);

		for (int i = 0; i < charactersInText.Length; i++)
		{
			if (!_alreadyDisplayingText)
			{
				exitNoReset = true;
				break;
			}

			if (charactersInText[i] == StartKey)
			{
				isSpecialKey = true;
				continue;
			}

			if (charactersInText[i] == EndKey && isSpecialKey)
			{
				if (isPauseDuration)
				{
					float pauseDuration = float.Parse(timeDurationAsText);

					isPauseDuration = false;

					yield return new WaitForSeconds(pauseDuration);

					if (!_alreadyDisplayingText)
					{
						exitNoReset = true;
						break;
					}
				}
				else if (isCharacterSpeed)
				{
					isCharacterSpeed = false;

					textSpeed = float.Parse(timeDurationAsText);
				}
				else if (isClearKey)
				{
					isClearKey = false;

					currentText = "";
				}

				isSpecialKey = false;

				continue;
			}

			if (isSpecialKey)
			{
				if (charactersInText[i] == PauseTimingKey)
				{
					timeDurationAsText = "";
					isPauseDuration = true;
					continue;
				}
				else if (charactersInText[i] == CharacterTimingKey)
				{
					timeDurationAsText = "";
					isCharacterSpeed = true;
					continue;
				}
				else if (charactersInText[i] == ClearKey)
				{
					isClearKey = true;
					continue;
				}

				timeDurationAsText += charactersInText[i];

				continue;

			}

			if (!_alreadyDisplayingText)
			{
				exitNoReset = true;
				break;
			}

			currentText += charactersInText[i];
			SubtitleTextbox.text = currentText;
			yield return new WaitForSeconds(textSpeed);

			if (!_alreadyDisplayingText)
			{
				exitNoReset = true;
				break;
			}

		}

		if (!exitNoReset && _alreadyDisplayingText)
		{
			SubtitleTextbox.text = "";
			SubtitleTextbox.gameObject.SetActive(false);
			_alreadyDisplayingText = false;
		}
	}
}
