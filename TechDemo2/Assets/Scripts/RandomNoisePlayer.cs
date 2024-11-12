using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoisePlayer : MonoBehaviour
{
	public AudioClip[] AudioClips;

	public AudioSource audioSource;

	public float MinimumTimeTilNextEvent = 10f;
	public float MaximumTimeTilNextEvent = 60f;

	// public Transform Pos1;
	// public Transform Pos2;

	// public float LerpSpeed = 0.1f;

	// private bool _secondTarget = true;



	private float _time;

	// private float _lerpTime = 0;

	// Start is called before the first frame update
	void Start()
	{
		_time = Random.Range(MinimumTimeTilNextEvent, MaximumTimeTilNextEvent);
	}

	// Update is called once per frame
	void Update()
	{
		if (audioSource == null || AudioClips.Length <= 0) return;

		if (_time >= 0)
		{
			_time -= Time.deltaTime;
		}

		if (_time <= 0)
		{
			audioSource.PlayOneShot(AudioClips[Random.Range(0, AudioClips.Length - 1)]);

			_time = Random.Range(MinimumTimeTilNextEvent, MaximumTimeTilNextEvent);
		}
	}
}
