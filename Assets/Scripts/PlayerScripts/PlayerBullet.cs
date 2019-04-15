using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

	public int speed;
	public Rigidbody2D rb;
	public GameObject shotExplode;

	void Start()
	{
		Destroy(gameObject, 1.2f);
		rb.velocity = transform.up * speed;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Enemy enemy = other.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(1);
			speed = 0;
			Instantiate(shotExplode, transform.position, transform.rotation);
			Destroy(gameObject);
		}
		//Explode when colliding with anything except the layer to be ignored
		if (!(other.gameObject.layer == 5))
		{
		speed = 0;
		Instantiate(shotExplode, transform.position, transform.rotation);
		Destroy(gameObject);
		}

	}

}