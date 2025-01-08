using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFlicker : MonoBehaviour
{
	public Light LightSource;

	public float MaxOnTime = 1;
	public float MinOnTime = 0;
	public float MaxOffTime = 1;
	public float MinOffTime = 0;

	private float _localTime = 0;
	private float _waitTime = 0;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		_localTime += Time.deltaTime;

		if (_localTime >= _waitTime)
		{
			LightSource.enabled = !LightSource.enabled;



			if (LightSource.enabled)
			{
				_waitTime = Random.Range(MinOnTime, MaxOnTime);


			}
			else
			{
				_waitTime = Random.Range(MinOffTime, MaxOffTime);


			}

			_localTime = 0;
		}
	}
}
