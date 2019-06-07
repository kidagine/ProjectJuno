﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

	private static AnimationEventHandler instance = null;
	private string activate = "";
	private string release = "";

	public static AnimationEventHandler Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
			return;
		}
		else
		{
			instance = this;
		}
	}

	public void SetActivate(string stringValue)
	{
		activate = stringValue;
	}

	public bool GetActivate()
	{
		if (activate.Equals("Activate"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void SetRelease(string stringValue)
	{
		release = stringValue;
	}

	public bool GetRelease()
	{
		if (release.Equals("Release"))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}
