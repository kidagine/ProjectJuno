using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEvent : MonoBehaviour {

	public GameObject[] eventObjects;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			foreach (GameObject eventObject in eventObjects)
			{
				eventObject.SetActive(true);
			}
		}
	}

}
