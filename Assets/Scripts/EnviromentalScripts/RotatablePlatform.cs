using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatablePlatform : MonoBehaviour {

	public GameObject player;

	void Start()
	{

	}

	void Update ()
	{
		transform.Rotate(0, 0, 1);
	}
}
