using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public PlayerStats playerStats;
	public Transform firePoint;
	public GameObject bulletPrefab;
	public SpriteRenderer sprite;

	private Color enemyColor;
	private float cooldown = 0.8f;
	private int health = 5;
	private int expValue = 1;

	void Start()
	{
		ColorUtility.TryParseHtmlString("#ff006f", out enemyColor);
	}

	void Update()
	{
		if (cooldown <= 0)
		{
			Shoot();
			cooldown = 1.2f;
		}
		cooldown -= Time.deltaTime;
	}

	void Shoot()
	{
		FindObjectOfType<AudioManager>().Play("Shot");
		Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
	}

	public void TakeDamage(int damage)
	{
		sprite.color = Color.white;

		health -= damage;
		if (health <= 0)
		{
			playerStats.receiveExp(expValue);
			Destroy(gameObject);
		}
		Invoke("ResetColor", 0.2f);
	}

	void ResetColor()
	{
		sprite.color = enemyColor;
	}

}
