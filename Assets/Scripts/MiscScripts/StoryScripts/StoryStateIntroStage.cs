using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStateIntroStage : MonoBehaviour
{

	[SerializeField] private Animator fadePanelAnimator;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private GameObject npc;


	void Start()
    {
		playerAnimator.SetTrigger("Lay");
		fadePanelAnimator.SetTrigger("FadeIntroStage");
		playerMovement.enabled = false;
    }

	void Update()
	{

		if (AnimationEventHandler.Instance.stringValue.Equals("Activate"))
		{
			npc.SetActive(true);
		}
		else if (AnimationEventHandler.Instance.stringValue.Equals("Deactivate"))
		{
			npc.SetActive(false);
		}
	}

	public void ShowNPC()
	{
		Debug.Log("TEST");
		npc.SetActive(true);
	}

	public void HideNPC()
	{
		npc.SetActive(false);
	}

}
