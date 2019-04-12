using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour {

	public Slider levelProgressSlider;
	public Transform player;
	public Transform endPoint;

	private float maxDistance;
	private float remainingDistance;
	private float distanceDone;

	void Start ()
	{
		maxDistance = Vector3.Distance(player.position, endPoint.position);
		levelProgressSlider.maxValue = Mathf.RoundToInt(maxDistance);
	}
	
	void Update ()
	{
		if (!(player == null || endPoint == null))
		remainingDistance = Vector3.Distance(player.position, endPoint.position);
		distanceDone = maxDistance - remainingDistance;
		levelProgressSlider.value = distanceDone;
	}
}
