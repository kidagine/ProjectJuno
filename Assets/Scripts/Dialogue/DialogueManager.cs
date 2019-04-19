using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public PlayerMovement playerMovement;
	public PlayerAim playerAim;
	public GameObject dialoguePane;
	public GameObject dialogueArrow;
	public Animator dialoguePaneAnimator;
	public Text textDisplay;
	public Queue<string> sentences;
	public float typingSpeed;

	private bool isDialogueTyping;
	private bool sentenceFinished;
	private string currentSentence;
	private string sentence;


	void Start()
	{
		sentences = new Queue<string>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.J) && sentenceFinished == true)
		{
			isDialogueTyping = true;
			sentenceFinished = false;
			dialogueArrow.SetActive(false);
			DisplayNextSentence(1);
		}
		else if (Input.GetKeyDown(KeyCode.J) && isDialogueTyping == true)
		{
			StopAllCoroutines();
			isDialogueTyping = false;
			sentenceFinished = true;
			textDisplay.text = "";
			textDisplay.text = currentSentence;
			dialogueArrow.SetActive(true);
		}
	}

	public void StartDialogue (Dialogue dialogue, int indexSkipToPass)
	{
		isDialogueTyping = true;
		dialoguePane.SetActive(true);
		dialoguePaneAnimator.SetTrigger("Open");
		DisableMovement();

		sentences.Clear();
		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		DisplayNextSentence(indexSkipToPass);
	}

	public void DisplayNextSentence(int indexSkipTo)
	{
		if (sentences.Count == 0)
		{
			EndDialog();
			return;
		}
		for (int i = 0; i < indexSkipTo; i++)
		{
			sentence = sentences.Dequeue();
		}
		currentSentence = sentence;
		StopAllCoroutines();
		StartCoroutine(Type(sentence));
	}

	IEnumerator Type(string sentence)
	{
		textDisplay.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			FindObjectOfType<AudioManager>().Play("Typing");
			textDisplay.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}
		sentenceFinished = true;
		dialogueArrow.SetActive(true);
	}

	public void EndDialog()
	{
		textDisplay.text = "";
		dialoguePaneAnimator.SetTrigger("Close");
		FindObjectOfType<DialogueTrigger>().hasDialogueStarted = false;
		StartCoroutine(DisablePane());
		EnableMovement();
	}

	IEnumerator DisablePane()
	{
		yield return new WaitForSeconds(0.4f);
		dialoguePaneAnimator.Rebind();
		dialoguePane.SetActive(false);
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
