using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStateIntroStage : MonoBehaviour
{

	[SerializeField] private Animator fadePanelAnimator;
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private Animator cameraAnimator;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private GameObject npc;


	void Start()
    {
		FindObjectOfType<AudioManager>().Play("LightFadeOut");
		FindObjectOfType<AudioManager>().Play("IntroStagePiano");
		cameraAnimator.SetTrigger("ZoomOut");
		playerAnimator.SetBool("IsLaying", true);
		playerAnimator.SetBool("CutsIsSleeping", true);
		fadePanelAnimator.SetTrigger("FadeIntroStage");
		playerMovement.enabled = false;
	}

	void Update()
	{
		if (FindObjectOfType<AnimationEventHandler>().GetChangeAnim())
		{
			playerAnimator.SetBool("CutsIsSleeping", false);
			playerAnimator.SetTrigger("OpenEyes");
		}

		if (FindObjectOfType<AnimationEventHandler>().GetRelease())
		{
			playerAnimator.SetTrigger("WakeUp");
			if (Input.anyKey)
			{
				playerMovement.enabled = true;
				playerAnimator.SetBool(("IsLaying"), false);
				Destroy(this);
			}
		}

		if (FindObjectOfType<AnimationEventHandler>().GetActivate())
		{
			npc.SetActive(true);
		}
		else if (!(FindObjectOfType<AnimationEventHandler>().GetActivate()))
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
