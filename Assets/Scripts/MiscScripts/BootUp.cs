using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootUp : MonoBehaviour {

	[Header("Animators")]
	[SerializeField] private Animator paneFadeAnimator;

	[Space]
	[Header("GameObjects")]
	[SerializeField] private GameObject paneFade;
	[SerializeField] private GameObject logoImage;
	[SerializeField] private GameObject gameTitleText;
	[SerializeField] private GameObject flyMyChildText;
	[SerializeField] private GameObject introVideo;

	void Start ()
	{
		StartCoroutine(PlayIntroFadeAndCheckSaveFile());
	}

	IEnumerator PlayIntroFadeAndCheckSaveFile()
	{
		yield return StartCoroutine(LogoFade());
		//TODO CHECK IF THERE IS A SAVE, IF YES GO TO MAIN MENU AND DO NOT CONTINUE THE FADING COROUTINE
		yield return StartCoroutine(GameTitleFade());
		yield return StartCoroutine(IntroLight());
		yield return StartCoroutine(GoToIntroScene());
	}

	private IEnumerator GoToIntroScene()
	{
		yield return new WaitForSeconds(22f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private IEnumerator LogoFade()
	{
		yield return new WaitForSeconds(5);
		paneFadeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1);
		logoImage.SetActive(false);
	}

	private IEnumerator GameTitleFade()
	{
		yield return new WaitForSeconds(2);
		gameTitleText.SetActive(true);
		paneFadeAnimator.SetTrigger("FadeIn");
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1);
		gameTitleText.SetActive(false);
	}

	private IEnumerator IntroLight()
	{
		yield return new WaitForSeconds(2);
		paneFadeAnimator.SetTrigger("FadeIn");
		flyMyChildText.SetActive(true);
		yield return new WaitForSeconds(2);
		FindObjectOfType<AudioManager>().Play("Intro");
		paneFade.SetActive(false);
		introVideo.SetActive(true);
		yield return new WaitForSeconds(4);
		flyMyChildText.SetActive(false);
	}

}
