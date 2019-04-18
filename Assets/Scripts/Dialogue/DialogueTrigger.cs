using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public GameObject DialoguePromptButton;
	public Dialogue dialogue;

	private bool hasDialogueStarted;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" || (other.gameObject.tag == "PlayerHead"))
		{
			DialoguePromptButton.SetActive(true);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" || (other.gameObject.tag == "PlayerHead"))
		{
			if (Input.GetKeyDown(KeyCode.J) && !hasDialogueStarted)
			{
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
				hasDialogueStarted = true;
				DialoguePromptButton.SetActive(false);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" || (other.gameObject.tag == "PlayerHead"))
		{
			DialoguePromptButton.SetActive(false);
		}
	}

}
