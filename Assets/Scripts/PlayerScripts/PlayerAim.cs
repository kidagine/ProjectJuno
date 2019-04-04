using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

	public Transform firePoint;
	public GameObject bulletPrefab;
	public CameraShaker cameraShaker;

	private PlayerMovement playerMovement;
	private float cooldown = 0.3f;
	void Start ()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement>();
	}

	void Update ()
	{
		PointingGun();
		Shooting();
		cooldown -= Time.deltaTime;
	}

	private void Shooting()
	{
		if (Input.GetMouseButton(0))
		{
			if (cooldown <= 0)
			{
				Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				FindObjectOfType<AudioManager>().Play("Shot");
				cameraShaker.CameraShake();	
				cooldown = 0.3f;
			}
		}
	}

	private void PointingGun()
	{
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint(mousePos);
		Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

		//Limit player's aim depending on if they are on a wall
		if (playerMovement.onRightWall)
		{
			if (direction.x <= 0)
			{
				transform.up = direction;
			}
		}
		else if (playerMovement.onLeftWall)
		{
			if (direction.x >= 0)
			{
				transform.up = direction;
			}
		}
		else if (playerMovement.onTopWall)
		{
			if (direction.y >= 0)
			{
				transform.up = direction;
			}
		}
		else if (playerMovement.onBottomWall)
		{
			if (direction.y <= 0)
			{
				transform.up = direction;
			}
		}
		else
		{
			transform.up = direction;
		}
	}
}
