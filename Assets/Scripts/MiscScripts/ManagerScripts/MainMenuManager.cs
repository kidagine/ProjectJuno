using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour, IMenuManager
{
	[Header("Menus")]
	[SerializeField] private GameObject continueMenu;
	[SerializeField] private GameObject newGameMenu;
	[SerializeField] private GameObject optionsMenu;
	[SerializeField] private GameObject quitMenu;

	private GameObject menuToShow;

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
				menuToShow.SetActive(false);
			}
		}
	}

	public void ShowMenu(GameObject menuToShow)
	{
		HideAllMenus();
		this.menuToShow = menuToShow;
		Transform firstMenuOption = menuToShow.transform.GetChild(1).GetChild(0).GetChild(0);

		menuToShow.SetActive(true);
		Debug.Log(firstMenuOption.gameObject.name);
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
		throw new System.NotImplementedException();
	}

	public void Select(GameObject buttonObject)
	{
		throw new System.NotImplementedException();
	}
}
