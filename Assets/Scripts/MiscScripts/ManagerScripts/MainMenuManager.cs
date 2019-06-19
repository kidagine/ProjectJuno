using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour, IMenuManager
{

	[Header("Menus")]
	[SerializeField] private GameObject continueMenu;
	[SerializeField] private GameObject newGameMenu;
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private GameObject quitMenu;

	private GameObject menuToShow;
	private GameObject itemLastSelected;
	private Button currentlySelectedButton;
	private Button currentlyHoveredButton;
	private Text currentlySelectedText;
	private Text lastSelectedText;
	private Text currentlyHoveredText;
	private Text lastHoveredText;
	private bool hasNoSelectedButton;



	void Start()
    {
		HideAllMenus();
    }

	void Update()
	{
		if (menuToShow != null)
		{
			if (Input.GetButtonDown("Start"))
			{
				CloseCurrentlyOpenMenu();
			}
		}
	}

	private void CloseCurrentlyOpenMenu()
	{
		menuToShow.SetActive(false);
		EventSystem.current.SetSelectedGameObject(itemLastSelected);
	}

	public void ShowMenu(GameObject menuToShow)
	{
		HideAllMenus();
		itemLastSelected = EventSystem.current.currentSelectedGameObject;
		this.menuToShow = menuToShow;
		Transform firstMenuOption = menuToShow.transform.GetChild(1).GetChild(1).GetChild(0);

		menuToShow.SetActive(true);
		EventSystem.current.SetSelectedGameObject(firstMenuOption.gameObject);
	}

	private void HideAllMenus()
	{
		continueMenu.SetActive(false);
		newGameMenu.SetActive(false);
		optionsMenu.SetActive(false);
		quitMenu.SetActive(false);
	}

	public void Hover(GameObject buttonObject)
	{
		Button button = buttonObject.GetComponent<Button>();
		Text text = buttonObject.GetComponent<Text>();

		currentlySelectedText.color = Color.gray;
		text.color = Color.white;


		if (currentlyHoveredButton != null)
		{
			lastHoveredText = currentlyHoveredText;
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

	//Options menu
	public void SetBackgroundMusic(Text backgroundMusicText)
	{
		//if (isBackgroundMusicOn)
		//{
		//	backgroundMusic.Pause();
		//	isBackgroundMusicOn = false;
		//	backgroundMusicText.text = "bgm: off";
		//}
		//else if (!isBackgroundMusicOn)
		//{
		//	isBackgroundMusicOn = true;
		//	backgroundMusicText.text = "bgm: on";
		//}
	}

	public void SetVolume()
	{
		AudioListener.volume -= 0.1f;
		if (AudioListener.volume <= -0.1f)
		{
			AudioListener.volume = 1;
		}
	}

	public void SetInput(Text inputText)
	{
		//if (isUsingController)
		//{
		//	isUsingController = false;
		//	inputText.text = "input: kb/m";
		//}
		//else if (!isUsingController)
		//{
		//	isUsingController = true;
		//	inputText.text = "input: pad";
		//}
	}

	public void SetFullscreen(Text fullscreenText)
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

	public void QuitGame()
	{
		FindObjectOfType<AudioManager>().Play("Click");
		Application.Quit();
	}

	public void CancelConfirmation()
	{
		CloseCurrentlyOpenMenu();
	}
	//New game menu

}
