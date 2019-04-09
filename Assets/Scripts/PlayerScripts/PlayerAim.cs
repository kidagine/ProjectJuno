using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerAim : MonoBehaviour {

	public Transform firePoint;
	public GameObject bulletPrefab;
	public CameraShaker cameraShaker;
	public Slider gunClipSlider;
	public Animator gunClipSliderAnim;

	private PlayerMovement playerMovement;
	private float cooldown = 0.3f;
	private int gunClip = 10;
	private bool isReloading;

	void Start()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement>();
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
		if (Input.GetButton("Fire1") && !PauseManager.GameIsPaused)
		{
			if (cooldown <= 0 && gunClip >= 1 && !isReloading)
			{
				gunClip--;
				gunClipSlider.value -= 1;
				Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				FindObjectOfType<AudioManager>().Play("Shot");
				cameraShaker.CameraShake();
				cooldown = 0.3f;
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

		if (Input.GetKey(KeyCode.R) && gunClip != 10 && !isReloading)
		{
			isReloading = true;
			FindObjectOfType<AudioManager>().Play("Reload");
			gunClipSliderAnim.enabled = true;
			gunClipSliderAnim.Play("Reload" ,- 1, 0f);
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
		//Using Mouse
		if (!PauseManager.isUsingController)
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos = Camera.main.ScreenToWorldPoint(mousePos);
			direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		}

		//Using Controller
		if (PauseManager.isUsingController)
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
