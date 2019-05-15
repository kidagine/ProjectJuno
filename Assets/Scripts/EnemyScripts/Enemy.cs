using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public PlayerStats playerStats;
	public GameObject enemyDeathPrefab;
	public GameObject bulletPrefab;
	public Transform firePoint;
	public SpriteRenderer sprite;
	public int health = 5;

	[HideInInspector] public float cooldown = 0.8f;

	private Color enemyColor;
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

	public virtual void Shoot()
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
			playerStats.ReceiveExp(expValue);
			FindObjectOfType<AudioManager>().Play("Death");
			Instantiate(enemyDeathPrefab, new Vector2(transform.position.x, transform.position.y+0.7f), Quaternion.identity);
			Destroy(gameObject);
		}
		Invoke("ResetColor", 0.1f);
	}

	void ResetColor()
	{
		sprite.color = enemyColor;
	}

}
