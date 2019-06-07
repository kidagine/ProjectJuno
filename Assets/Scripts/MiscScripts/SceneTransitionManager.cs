using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour {

	private static SceneTransitionManager instance = null;

	private GameObject player;
	private Vector3 lastPlayerPosition;

	private static bool hasPickedBoots;
	private GameObject boots;
	private GameObject playerAim;

	public static SceneTransitionManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		SceneManager.sceneLoaded += SceneManagersceneLoaded;

		if (instance != null)
		{
			Destroy(this);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this);
	}

	public void SetPlayerPosition(Vector3 playerPosition)
	{
		lastPlayerPosition = playerPosition;
	}

	public Vector3 GetPlayerPosition()
	{
		return lastPlayerPosition;
	}

	public void HasPickedItem(bool itemPicked)
	{
		hasPickedBoots = itemPicked;
	}

	private void SceneManagersceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		playerAim = GameObject.Find("PlayerAimPoint");

		if (playerAim != null)
		{
			if (!hasPickedBoots)
			{
				playerAim.SetActive(false);
			}
			else
			{
				playerAim.SetActive(true);
			}
		}

	}
}
