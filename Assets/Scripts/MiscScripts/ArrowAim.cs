using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAim : MonoBehaviour {

	private float cooldown = 0.3f;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		Move();
		cooldown -= Time.deltaTime;
	}

	private void Move()
	{
		if (cooldown <= 0)
		{
			cooldown = 0.2f;
		}
	}
}
