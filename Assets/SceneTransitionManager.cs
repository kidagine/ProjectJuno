using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour {

	private GameObject player;
	private Vector3 lastPlayerPosition;

	private static SceneTransitionManager instance = null;

	public static SceneTransitionManager Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}


	public void SetPlayerPosition(Vector3 playerPosition)
	{
		lastPlayerPosition = playerPosition;
	}

	public Vector3 GetPlayerPosition()
	{
		return lastPlayerPosition;
	}

}
