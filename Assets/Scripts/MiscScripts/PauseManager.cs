using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour {

	public static bool GameIsPaused;
	public static bool GameIsOver;
	public static bool isUsingController;
	public GameObject pauseMenuUI;
	public GameObject generalMenuUI;
	public GameObject optionsMenuUI;
	public GameObject gameOverUi;
	public Text backgroundMusicText;
	public Text inputText;
	public Text fullscreenText;
	public AudioSource backgroundMusic;
	public Button resumeButton;
	public PixelBoy pixelBoy;

	private bool isBackgroundMusicOn = true;
	private bool hasBegun = false;

	//PauseState
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
		if (GameIsOver)
		{
			StartCoroutine(EndGame());
		}
		else if (!GameIsOver && !hasBegun)
		{
			BeginGame();
		}
	}

	public void Resume()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		resumeButton.Select();
		pauseMenuUI.SetActive(false);
		generalMenuUI.SetActive(false);	
		Time.timeScale = 0.2f;
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

	//GameState


	IEnumerator EndGame()
	{
		gameOverUi.SetActive(true);
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		GameIsOver = false;
	}

	private void BeginGame()
	{
		hasBegun = true;
		pixelBoy.IncreaseResolution(318);
	}

}
