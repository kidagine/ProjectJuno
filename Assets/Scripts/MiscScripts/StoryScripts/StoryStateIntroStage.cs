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
		playerAnimator.SetBool(("IsLaying"), true);
		fadePanelAnimator.SetTrigger("FadeIntroStage");
		playerMovement.enabled = false;
    }

	void Update()
	{
		if (AnimationEventHandler.Instance.GetRelease())
		{
			if (Input.anyKey)
			{
				playerMovement.enabled = true;
				playerAnimator.SetBool(("IsLaying"), false);
				Destroy(this);
			}
		}

		if (AnimationEventHandler.Instance.GetActivate())
		{
			npc.SetActive(true);
		}
		else if (!(AnimationEventHandler.Instance.GetActivate()))
		{
			npc.SetActive(false);
		}
	}

	public void ShowNPC()
	{
		npc.SetActive(true);
	}

	public void HideNPC()
	{
		npc.SetActive(false);
	}

}
