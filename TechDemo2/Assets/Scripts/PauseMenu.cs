using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public static PauseMenu Instance { get; private set; }

	public bool LockEscape = false;

	public bool IsPaused { get; private set; } = false;

	public GameObject PauseMenuGameObject;

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

	}

	// Update is called once per frame
	void Update()
	{
		if (LockEscape && IsPaused && Input.GetKeyDown(KeyCode.Escape))
		{
			IsPaused = false;
			PauseMenuGameObject.SetActive(IsPaused);
			return;
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			IsPaused = !IsPaused;
		}

		PauseMenuGameObject.SetActive(IsPaused);
	}

	public void Resume()
	{
		IsPaused = false;
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
