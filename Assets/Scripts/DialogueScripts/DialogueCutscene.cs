using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueCutscene : MonoBehaviour {

	public Text cutsceneText;
	public string[] sentences;

	private Queue<string> sentenceQueue;
	private int index;
	private string sentence;

	void Start()
	{
		sentenceQueue = new Queue<string>();
	}

	public void ChangeCutsceneText()
	{
		sentenceQueue.Clear();
		foreach (string sentence in sentences)
		{
			sentenceQueue.Enqueue(sentence);
		}
		index++;
		DisplayNextSentence(index);
	}

	private void DisplayNextSentence(int index)
	{
		if (sentenceQueue.Count == 0)
		{
			return;
		}
		for (int i = 0; i < index; i++)
		{
			sentence = sentenceQueue.Dequeue();
		}
		StopAllCoroutines();
		StartCoroutine(Type(sentence));
	}

	IEnumerator Type(string sentence)
	{
		cutsceneText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			FindObjectOfType<AudioManager>().Play("Typing");
			cutsceneText.text += letter;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
