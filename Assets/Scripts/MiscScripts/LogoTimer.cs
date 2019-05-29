using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoTimer : MonoBehaviour {

	public Animator paneFadeAnimator;
	public GameObject logo;
	public GameObject gameTitle;
	public GameObject flyMyChildText;
	public float timer;

	void Start ()
	{
		StartCoroutine(ChangeScene());
	}

	IEnumerator ChangeScene()
	{
		yield return new WaitForSeconds(5);
		paneFadeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1);
		logo.SetActive(false);

		yield return new WaitForSeconds(2);
		gameTitle.SetActive(true);
		paneFadeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1);
		gameTitle.SetActive(false);

		yield return new WaitForSeconds(2);
		flyMyChildText.SetActive(true);
		paneFadeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1);
		flyMyChildText.SetActive(false);
		//yield return new WaitForSeconds(10	);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
