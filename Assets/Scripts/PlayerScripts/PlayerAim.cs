using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour {

	public Transform firePoint;
	public GameObject bulletPrefab;
	public CameraShaker cameraShaker;
	public static bool isUsingController = false;

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
		if (Input.GetButton("Fire1") && !PauseManager.GameIsPaused)
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
		Vector2 direction = Vector2.zero;
		//Using Mouse
		if (!isUsingController)
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		}

		//Using Controller
		if (isUsingController)
		{
			float directionX = Input.GetAxisRaw("PadHorizontal");
			float directionY = Input.GetAxisRaw("PadVertical");
			direction = new Vector2(directionX, directionY);
		}

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
