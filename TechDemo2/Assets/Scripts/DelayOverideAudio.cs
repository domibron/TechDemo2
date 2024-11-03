using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayOverideAudio : MonoBehaviour
{

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void StartDelay()
	{
		StopAllCoroutines();
		StartCoroutine(Delay());
	}

	IEnumerator Delay()
	{
		yield return new WaitForSeconds(2f);
		AIVoiceManager.Instance.OverrideFailed();
	}
}
