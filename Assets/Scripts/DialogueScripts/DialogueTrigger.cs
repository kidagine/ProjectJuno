using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public GameObject DialoguePromptButton;
	public Dialogue dialogue;
	public int indexSkipTo;

	public bool hasDialogueStarted;
	private bool hasDialogueBeenSaid;

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
			if (Input.GetKey(KeyCode.K) && !hasDialogueStarted && !hasDialogueBeenSaid)
			{
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue, 1);
				hasDialogueStarted = true;
				hasDialogueBeenSaid = true;
				DialoguePromptButton.SetActive(false);
			}
			else if (Input.GetKey(KeyCode.K) && !hasDialogueStarted && hasDialogueBeenSaid)
			{
				FindObjectOfType<DialogueManager>().StartDialogue(dialogue, indexSkipTo);
				hasDialogueStarted = true;
				DialoguePromptButton.SetActive(false);
			}
			else if (!hasDialogueStarted)
				DialoguePromptButton.SetActive(true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" || (other.gameObject.tag == "PlayerHead"))
		{
			DialoguePromptButton.SetActive(false);
			hasDialogueStarted = false;
		}
	}

}
