using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoAim : Enemy {

	public GameObject player;
	private float cooldown = 0.8f;
	Vector2 direction;

	void Update()
	{
		if (player != null)
		{
			direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
			transform.up = direction;
			if (cooldown <= 0)
			{
				Shoot();
				cooldown = 1.2f;
			}
			cooldown -= Time.deltaTime;
		}
	}

	public override void Shoot()
	{
		base.Shoot();
		Debug.Log("test");
	}
}
