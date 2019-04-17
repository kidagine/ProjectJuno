using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour {

	public PlayerMovement playerMovement;
	public PlayerAim playerAim;
	public GameObject dialogPane;
	public Text textDisplay;
	public string[] sentences;
	public float typingSpeed;

	private int index = 0;
	private int lastIndex;
	private bool dialogStarted;


	void Update()
	{
		if (Input.anyKeyDown && dialogStarted)
		{
			if (index < sentences.Length - 1)
			{
				index++;
				textDisplay.text = "";
				StartCoroutine(Type());
			}
			else
			{
				textDisplay.text = "";
			}
		}
	}

	public void StartDialog(int lastIndex)
	{
		Debug.Log("test");
		this.lastIndex = lastIndex;
		dialogStarted = true;
		dialogPane.SetActive(true);
		DisableMovement();
		StartCoroutine(Type());
	}

	private void DisableMovement()
	{
		playerMovement.enabled = false;
		playerAim.enabled = false;
	}

	IEnumerator Type()
	{
		foreach (char letter in sentences[index].ToCharArray())
		{
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
		//if (index < sentences.Length - 1)
		//{
		//	EndDialog();
		//}
	}

	public void EndDialog()
	{
		dialogPane.SetActive(false);
		EnableMovement();
	}

	private void EnableMovement()
	{
		playerMovement.enabled = true;
		playerAim.enabled = true;
	}

}
