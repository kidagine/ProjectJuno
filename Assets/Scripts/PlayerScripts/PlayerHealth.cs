using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public Slider currentHealthSlider;
	public Text currentHealthTxt;
	public int health = 3;

	void Start()
	{
		currentHealthSlider.maxValue = health;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Deadly"))
		{
			health--;
			currentHealthSlider.value = health;
			currentHealthTxt.text = health + "";
		}
	}

}
