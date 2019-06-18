using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAim : MonoBehaviour
{

	public PlayerStats playerStats;
	public PlayerMovement playerMovement;
	public CameraShaker cameraShaker;
	public Transform firePoint;
	public GameObject bulletPrefab;
	public GameObject beam;
	public SpriteRenderer playerSprite;
	public Slider gunClipSlider;
	public Animator gunClipSliderAnim;
	public bool isMovePossible;

	private float cooldown = 0.3f;
	private int gunClip = 10;
	private bool isReloading;
	private bool isRotatable;

	void Start()
	{
		isMovePossible = true;
		gunClipSlider.maxValue = gunClip;
		gunClipSlider.value = gunClip;
	}

	void Update()
	{
		PointingGun();
		Shooting();
	
		cooldown -= Time.deltaTime;
	}

	private void Shooting()
	{
		if (Input.GetButton("Fire1") && !PauseMenuManager.GameIsPaused && bulletPrefab != null && !playerMovement.isMoving)
		{
			if (cooldown <= 0 && gunClip >= 1 && !isReloading)
			{
				gunClip--;
				gunClipSlider.value -= 1;
				FindObjectOfType<AudioManager>().Play("Shot");
				Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				playerStats.playerShield.SetActive(false);
				cooldown = 0.25f;
			}
			else if (gunClip <= 0 && !isReloading)
			{
				isReloading = true;
				FindObjectOfType<AudioManager>().Play("Reload");
				gunClipSliderAnim.enabled = true;
				gunClipSliderAnim.Play("Reload", -1, 0f);
				StartCoroutine(Reload());
			}
		}
		else if (Input.GetKey(KeyCode.R) && gunClip != 10 && !isReloading)
		{
			isReloading = true;
			FindObjectOfType<AudioManager>().Play("Reload");
			gunClipSliderAnim.enabled = true;
			gunClipSliderAnim.Play("Reload", -1, 0f);
			StartCoroutine(Reload());
		}
	}
	IEnumerator Reload()
	{
		yield return new WaitForSeconds(1);
		gunClip = 10;
		gunClipSlider.value = gunClip;
		gunClipSliderAnim.enabled = false;
		isReloading = false;
	}

	private void PointingGun()
	{
		Vector2 direction = Vector2.zero;
		Vector2 directionOutBounds = Vector2.zero;

		//Using Mouse
		if (!PauseMenuManager.isUsingController)
		{
			Vector2 mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		}

		//Using Controller
		if (PauseMenuManager.isUsingController)
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
			else
			{
				DirectionOutOfBoundsHorizontal(direction, directionOutBounds);
			}
			FlipPlayerOnHorizontal(direction);
		}
		else if (playerMovement.onLeftWall)
		{
			if (direction.x >= 0)
			{
				transform.up = direction;
			}
			else
			{
				DirectionOutOfBoundsHorizontal(direction, directionOutBounds);
			}
			FlipPlayerOnHorizontal(direction);
		}
		else if (playerMovement.onTopWall)
		{
			if (direction.y >= 0)
			{
				transform.up = direction;
			}
			else
			{
				DirectionOutOfBoundsVertical(direction, directionOutBounds);
			}
		}
		else if (playerMovement.onBottomWall)
		{
			if (direction.y <= 0)
			{
				transform.up = direction;
			}
			else
			{
				DirectionOutOfBoundsVertical(direction, directionOutBounds);
			}
			FlipPlayerOnVertical(direction);
		}
		else if (playerMovement.onRotatable)
		{
			if (!isRotatable)
			{
				transform.up = direction;
				isMovePossible = true;
			}
			else
			{
				transform.up = direction;
				isMovePossible = false;
				beam.SetActive(false);
			}
		}
		else
		{
			transform.up = direction;
		}
	}

	private void FlipPlayerOnVertical(Vector2 direction)
	{
		if (direction.x <= 0.1)
			playerSprite.flipX = false;
		else if (direction.x >= 0.1)
			playerSprite.flipX = true;
	}

	private void FlipPlayerOnHorizontal(Vector2 direction)
	{
		if (playerMovement.onLeftWall)
		{
			if (direction.y <= 0.1)
				playerSprite.flipX = false;
			else if (direction.y >= 0.1)
				playerSprite.flipX = true;
		}
		else if (playerMovement.onRightWall)
		{
			if (direction.y <= 0.1)
				playerSprite.flipX = true;
			else if (direction.y >= 0.1)
				playerSprite.flipX = false;
		}
	}

	private void DirectionOutOfBoundsHorizontal(Vector2 direction, Vector2 directionOutBounds)
	{
		float directionOutBoundsY = direction.y;
		float directionOutBoundsX = direction.x * -1;
		directionOutBounds = new Vector2(directionOutBoundsX, directionOutBoundsY);
		transform.up = directionOutBounds;
	}

	private void DirectionOutOfBoundsVertical(Vector2 direction, Vector2 directionOutBounds)
	{
		float directionOutBoundsY = direction.y * -1;
		float directionOutBoundsX = direction.x;
		directionOutBounds = new Vector2(directionOutBoundsX, directionOutBoundsY);
		transform.up = directionOutBounds;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (playerMovement.onRotatable)
		{
			isRotatable = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		isRotatable = false;
	}

}
