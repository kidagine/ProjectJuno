using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public SpriteRenderer sprite;

	private Color enemyColor;
	private int health = 10;

	void Start()
	{
		ColorUtility.TryParseHtmlString("#ff006f", out enemyColor);
	}

	public void TakeDamage(int damage)
	{
		sprite.color = Color.white;

		health -= damage;
		if (health <= 0)
		{
			Destroy(gameObject);
		}
		Invoke("ResetColor", 0.2f);
	}

	void ResetColor()
	{
		sprite.color = enemyColor;
	}

}
