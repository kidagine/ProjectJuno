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
		FindObjectOfType<AudioManager>().Play("LightFadeOut");
		playerAnimator.SetBool("IsLaying", true);
		playerAnimator.SetBool("CutsIsSleeping", true);
		fadePanelAnimator.SetTrigger("FadeIntroStage");
		playerMovement.enabled = false;
		Invoke("PlayBackgroundSound", 1);
	}

	void Update()
	{
		if (AnimationEventHandler.Instance.GetChangeAnim())
		{
			Debug.Log("se");
			playerAnimator.SetBool("CutsIsSleeping", false);
			playerAnimator.SetTrigger("OpenEyes");
		}

		if (AnimationEventHandler.Instance.GetRelease())
		{
			playerAnimator.SetTrigger("WakeUp");
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

	private void PlayBackgroundSound()
	{
		FindObjectOfType<AudioManager>().Play("BackgroundIntroStage");
	}

}
