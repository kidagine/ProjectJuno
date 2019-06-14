using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStateIntroStage : MonoBehaviour
{
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private Animator cameraAnimator;
	[SerializeField] private PlayerMovement playerMovement;

	void Start()
    {
		playerAnimator.SetBool("IsLaying", true);
		playerAnimator.SetTrigger("OpenEyes");
		playerAnimator.SetTrigger("WakeUp");
		cameraAnimator.SetTrigger("ZoomOut");
		playerMovement.enabled = false;
	}

	void Update()
	{
		if (FindObjectOfType<TimelineSignalInvoker>().TimelineIsOver())
		{
			if (Input.anyKey)
			{
				playerAnimator.SetBool("IsLaying", false);
				playerMovement.enabled = true;
				Destroy(this);
			}
		}
	}

}
