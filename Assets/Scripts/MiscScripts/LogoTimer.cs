using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoTimer : MonoBehaviour {

	public Animator paneFadeAnimator;
	public float timer;

	void Start ()
	{
		paneFadeAnimator.SetTrigger("FadeOut");
		Invoke("ChangeScene", timer);
	}

	void ChangeScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
