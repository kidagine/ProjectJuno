using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineSignalInvoker : MonoBehaviour
{
	private bool boolValue;
	private bool isTimelineOver;

	public void SetBoolValue(bool boolValue)
	{
		this.boolValue = boolValue;
	}

	public void PlayerMovementActivation(PlayerMovement playerMovement)
	{
		playerMovement.enabled = boolValue;
	}

	public void SetTimelineIsOver(bool boolValue)
	{
		isTimelineOver = boolValue;
	}

	public bool TimelineIsOver()
	{
		return isTimelineOver;
	}

}
