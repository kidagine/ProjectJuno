using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

	private static AnimationEventHandler instance = null;
	private string activate = "";
	private string release = "";
	private string anim = "";

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

	public void SetChangeAnim(string stringValue)
	{
		anim = stringValue;
	}

	public bool GetChangeAnim()
	{
		if (anim.Equals("Change"))
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

	public void PlaySound(string sound)
	{
		FindObjectOfType<AudioManager>().Play(sound);
	}

}
