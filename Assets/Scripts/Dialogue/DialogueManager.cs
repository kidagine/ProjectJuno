using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public PlayerMovement playerMovement;
	public PlayerAim playerAim;
	public GameObject dialoguePane;
	public GameObject dialogueArrow;
	public Text textDisplay;
	public Queue<string> sentences;
	public float typingSpeed;

	private bool isDialogueTyping;
	private bool sentenceFinished;
	private string currentSentence;


	void Start()
	{
		sentences = new Queue<string>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J) && sentenceFinished == true)
		{
			isDialogueTyping = true;
			DisplayNextSentence();
			sentenceFinished = false;
			dialogueArrow.SetActive(false);
		}
		if (Input.GetKeyDown(KeyCode.J) && isDialogueTyping == true)
		{
			isDialogueTyping = false;
			StopAllCoroutines();
			textDisplay.text = "";
			textDisplay.text = currentSentence;
			sentenceFinished = true;
		}
	}

	public void StartDialogue (Dialogue dialogue)
	{
		dialoguePane.SetActive(true);
		DisableMovement();

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialog();
			return;
		}

		string sentence = sentences.Dequeue();
		currentSentence = sentence;
		StopAllCoroutines();
		StartCoroutine(Type(sentence));
	}

	IEnumerator Type(string sentence)
	{
		textDisplay.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
		sentenceFinished = true;
		dialogueArrow.SetActive(true);
	}

	public void EndDialog()
	{
		textDisplay.text = "";
		dialoguePane.SetActive(false);
		EnableMovement();
	}

	private void DisableMovement()
	{
		playerMovement.enabled = false;
		playerAim.enabled = false;
	}

	private void EnableMovement()
	{
		playerMovement.enabled = true;
		playerAim.enabled = true;
	}

}
