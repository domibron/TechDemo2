using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVoiceManager : MonoBehaviour
{
	public static AIVoiceManager Instance;

	public AudioClip IntoClip;

	[TextArea(1, 500)]
	public string IntoSubtitle;

	public AudioClip OverrideCommandClip;

	[TextArea(1, 500)]
	public string OverrideCommandSubtitle;

	public AudioClip AccessDeniedClip;

	[TextArea(1, 500)]
	public string AccessDeniedSubtitle;

	public AudioClip AccessGrantedClip;

	[TextArea(1, 500)]
	public string AccessGrantedSubtitle;

	private AudioSource _audioSource;

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
	}

	// Start is called before the first frame update
	void Start()
	{
		_audioSource = GetComponent<AudioSource>();

		SubtitleManger.Instance.DisplayText(IntoSubtitle);
		_audioSource.PlayOneShot(IntoClip);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void AccessGranted()
	{
		SubtitleManger.Instance.DisplayText(AccessGrantedSubtitle);
		_audioSource.PlayOneShot(AccessGrantedClip);
	}

	public void AccessDenied()
	{
		SubtitleManger.Instance.DisplayText(AccessDeniedSubtitle);
		_audioSource.PlayOneShot(AccessDeniedClip);
	}

	public void OverrideFailed()
	{
		SubtitleManger.Instance.DisplayText(OverrideCommandSubtitle);
		_audioSource.PlayOneShot(OverrideCommandClip);
	}

}
