using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject pauseMenuUI;
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
		}
	}

	public void Resume()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		pauseMenuUI.SetActive(false);	
		Time.timeScale = 1;
		GameIsPaused = false;
	}

	public void Pause()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0;
		GameIsPaused = true;
	}

	public void QuitGame()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		Application.Quit();
	}

	public void Hover()
	{
		FindObjectOfType<AudioManager>().Play("Hover");
	}
}
