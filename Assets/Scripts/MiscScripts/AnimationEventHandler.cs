using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

	private static AnimationEventHandler instance = null;
	[HideInInspector] public string stringValue;

	public static AnimationEventHandler Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		if (instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void SetActivate(string value)
	{
		stringValue = value;
	}

}
