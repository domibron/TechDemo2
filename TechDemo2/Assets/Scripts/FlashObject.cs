using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashObject : MonoBehaviour
{
	public GameObject ObjectToFlash;

	public float SecondsToFlash;

	private float _time = 0;

	// Start is called before the first frame update
	void Start()
	{
		_time = SecondsToFlash;
	}

	// Update is called once per frame
	void Update()
	{
		if (_time >= 0)
		{
			_time -= Time.deltaTime;
		}

		if (_time <= 0)
		{
			ObjectToFlash.SetActive(!ObjectToFlash.activeSelf);
			_time += SecondsToFlash;
		}
	}
}
