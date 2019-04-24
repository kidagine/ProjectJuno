using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoTimer : MonoBehaviour {

	public int timer;

	void Start ()
	{
		Invoke("ChangeScene", timer);
	}

	void ChangeScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
