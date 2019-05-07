using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAutoAim : Enemy {

	public GameObject player;
	public SpriteRenderer bossEye;
	Vector2 direction;

	private Color activeAimColor;
	private Color disabledAimColor;
	private bool isEyeActive;
	private float lerpValue = 0;

	void Start()
	{
		ColorUtility.TryParseHtmlString("#ff006f", out activeAimColor);
		ColorUtility.TryParseHtmlString("#ffffff", out disabledAimColor);
		bossEye.color = disabledAimColor;
	}

	void Update()
	{
		if (!isEyeActive)
		{
			bossEye.color = Color.Lerp(disabledAimColor, activeAimColor, Mathf.PingPong(Time.time, lerpValue));
			if (lerpValue < 1)
				lerpValue += Time.deltaTime / 1.0f;
			else
			{
				isEyeActive = true;
				lerpValue = 0.0f;
			}
		}

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
	}

}
