using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour {

	public static bool GameIsPaused;
	public static bool GameIsOver;
	public static bool isUsingController;
	public GameObject pauseMenuUI;
	public GameObject generalMenuUI;
	public GameObject optionsMenuUI;
	public GameObject gameOverUi;
	public Animator animator;
	public AnimatorUpdateMode animatorUpdateMode;
	public Button resumeButton;
	public Button bgmButton;
	public Text backgroundMusicText;
	public Text inputText;
	public Text fullscreenText;
	public AudioSource backgroundMusic;
	public PixelBoy pixelBoy;
	public int indexOfSceneToLoad;

	private Button currentlySelectedButton;
	private Button currentlyHoveredButton;
	private Text currentlySelectedText;
	private Text lastSelectedText;
	private Text currentlyHoveredText;
	private Text lastHoveredText;
	private bool hasNoSelectedButton;
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

		if (pauseMenuUI.activeSelf)
		{
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				if (currentlySelectedButton != null)
				{
					hasNoSelectedButton = true;
					currentlySelectedButton.Select();
					currentlySelectedText.color = Color.white;
				}
			}
		}
	}

	public void Resume()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		animator.SetTrigger("Close");
		if (isBackgroundMusicOn)
			backgroundMusic.Play();
		generalMenuUI.SetActive(false);
		optionsMenuUI.SetActive(false);
		Time.timeScale = 0.2f;
		GameIsPaused = false;
	}

	public void Pause()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		Invoke("ActivateGeneralMenu", 0.2f);
		backgroundMusic.Pause();
		pauseMenuUI.SetActive(true);
		generalMenuUI.SetActive(true);
		optionsMenuUI.SetActive(false);
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		animator.SetTrigger("Open");
		resumeButton.Select();
		Time.timeScale = 0;
		GameIsPaused = true;
	}

	private void ActivateGeneralMenu()
	{
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
		bgmButton.Select();
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

	public void Hover(GameObject buttonObject)
	{
		Button button = buttonObject.GetComponent<Button>();
		Text text = buttonObject.GetComponent<Text>();
		
		currentlySelectedText.color = Color.gray;
		text.color = Color.white;


		if (currentlyHoveredButton != null)
		{
			lastHoveredText= currentlyHoveredText;
			if (lastHoveredText != currentlyHoveredText)
			{
				lastHoveredText.color = Color.gray;
				FindObjectOfType<AudioManager>().Play("Hover");
			}
		}
		currentlyHoveredButton = button;
		currentlyHoveredText = text;
		currentlySelectedButton = currentlyHoveredButton;
		currentlySelectedButton.Select();
	}

	public void Select(GameObject buttonObject)
	{
		Button button = buttonObject.GetComponent<Button>();
		Text text = buttonObject.GetComponent<Text>();
		text.color = Color.white;

		if (currentlySelectedButton != null)
		{
			lastSelectedText = currentlySelectedText;
			lastSelectedText.color = Color.gray;
		}
		currentlySelectedButton = button;
		currentlySelectedText = text;

		currentlySelectedButton.Select();

		if (!hasNoSelectedButton)
		{
			FindObjectOfType<AudioManager>().Play("Hover");
		}
		hasNoSelectedButton = false;
	}

	//GameState
	IEnumerator EndGame()
	{
		gameOverUi.SetActive(true);
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene(indexOfSceneToLoad);
		GameIsOver = false;
	}

	private void BeginGame()
	{
		hasBegun = true;
		pixelBoy.IncreaseResolution(318);
	}

}
