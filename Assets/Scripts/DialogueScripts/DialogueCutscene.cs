using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueCutscene : MonoBehaviour {

	public Text cutsceneText;
	[TextArea] public string[] sentences;
	public float sentencesSpeed;
	public float[] speedarrays;

	private Queue<string> sentenceQueue;
	private int index;
	private int speedIndex;
	private string sentence;

	void Start()
	{
		speedIndex = 0;
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
		DisplayNextSentence(index, speedIndex);
	}

	private void DisplayNextSentence(int index, int speedIndex)
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
		StartCoroutine(Type(sentence,speedIndex));
	}

	IEnumerator Type(string sentence, int speedIndex)
	{
		cutsceneText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			FindObjectOfType<AudioManager>().Play("Typing");
			cutsceneText.text += letter;
			yield return new WaitForSeconds(speedarrays[speedIndex]);
		}
	}
}
