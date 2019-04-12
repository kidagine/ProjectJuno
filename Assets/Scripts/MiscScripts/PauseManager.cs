using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public static bool GameIsPaused = false;
	public static bool isUsingController = false;
	public GameObject pauseMenuUI;
	public GameObject generalMenuUI;
	public GameObject optionsMenuUI;
	public Text backgroundMusicText;
	public Text inputText;
	public Text fullscreenText;
	public AudioSource backgroundMusic;
	public Button resumeButton;

	private bool isBackgroundMusicOn = true;

	void Update ()
	{
		if (Input.GetButtonDown("Start"))
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
		resumeButton.Select();
		pauseMenuUI.SetActive(false);
		generalMenuUI.SetActive(false);	
		Time.timeScale = 1;
		GameIsPaused = false;
	}

	public void Pause()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		pauseMenuUI.SetActive(true);
		generalMenuUI.SetActive(true);
		optionsMenuUI.SetActive(false);
		Time.timeScale = 0;
		GameIsPaused = true;
	}

	public void QuitGame()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		Application.Quit();
	}

	public void OptionsGame()
	{
		generalMenuUI.SetActive(false);
		optionsMenuUI.SetActive(true);
	}

	public void BackgroundMusic()
	{
		if (isBackgroundMusicOn)
		{
			backgroundMusic.Pause();
			isBackgroundMusicOn = false;
			backgroundMusicText.text = "bgm: off";
		}
		else if (!isBackgroundMusicOn)
		{
			backgroundMusic.Play();
			isBackgroundMusicOn = true;
			backgroundMusicText.text = "bgm: on";
		}
	}

	public void SetInput()
	{
		if (isUsingController)
		{
			isUsingController = false;
			inputText.text = "input: kb/m";
		}
		else if (!isUsingController)
		{
			isUsingController = true;
			inputText.text = "input: pad";
		}
	}

	public void SetFullscreen()
	{
		if (Screen.fullScreen)
		{
			Screen.fullScreen = false;
			fullscreenText.text = "fullscreen: off";
		}
		else if (!Screen.fullScreen)
		{
			Screen.fullScreen = true;
			fullscreenText.text = "fullscreen: on";
		}
	}

	public void SetVolume()
	{	
		AudioListener.volume  -= 0.1f;
		if (AudioListener.volume <= -0.1f)
		{
			AudioListener.volume = 1;
		}
	}

	public void Hover()
	{
		FindObjectOfType<AudioManager>().Play("Hover");
	}

}
