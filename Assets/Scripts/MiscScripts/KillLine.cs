using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillLine : MonoBehaviour {

	public float speed;

	void Update ()
	{
		transform.Translate(0, Time.deltaTime/speed, 0);
	}
}
