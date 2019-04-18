using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public Rigidbody2D rb;
	public GameObject aimRay;
	public PlayerAimRayCast playerAimRay;
	public PlayerStats playerStats;

	[HideInInspector] public bool isMoving;
	[HideInInspector] public int speed;

	[HideInInspector] public bool onRightWall;
	[HideInInspector] public bool onLeftWall;
	[HideInInspector] public bool onTopWall;
	[HideInInspector] public bool onBottomWall;


	void Start ()
	{
		onTopWall = true;
		aimRay.SetActive(false);
	}

	void Update()
	{
		if (!Input.GetButton("Fire1"))
		{
			//Using KB/M
			if (!PauseManager.isUsingController)
			{
				if (Input.GetMouseButton(1))
				{
					aimRay.SetActive(true);
				}
				if (Input.GetMouseButtonUp(1) && !isMoving && playerAimRay.IsMovePossible())
				{
					ResetMovement();
				}
				if (Input.GetMouseButtonUp(1))
				{
					aimRay.SetActive(false);
				}
			}
			//Using Controller
			if (PauseManager.isUsingController)
			{
				if (Input.GetAxis("PadHorizontal") != 0 || Input.GetAxis("PadVertical") != 0)
				{
					aimRay.SetActive(true);
				}
				if (Input.GetButtonDown("Dash") && !isMoving && playerAimRay.IsMovePossible())
				{
					ResetMovement();
				}
				if (Input.GetAxis("PadHorizontal") == 0 && Input.GetAxis("PadVertical") == 0)
				{
					aimRay.SetActive(false);
				}
			}
		}
		else 
		{
			aimRay.SetActive(false);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("RightWall"))
		{
			rb.velocity = Vector2.zero;
			isMoving = false;
			onRightWall = true;
		}
		if (other.gameObject.CompareTag("LeftWall"))
		{
			rb.velocity = Vector2.zero;
			isMoving = false;
			onLeftWall = true;
		}
		if (other.gameObject.CompareTag("TopWall"))
		{
			rb.velocity = Vector2.zero;
			isMoving = false;
			onTopWall = true;
		}
		if (other.gameObject.CompareTag("BottomWall"))
		{
			rb.velocity = Vector2.zero;
			isMoving = false;
			onBottomWall = true;
		}
		if (other.gameObject.CompareTag("KillLine"))
		{
			Destroy(gameObject, 1F);
		}
	}

	private void ResetMovement()
	{
		FindObjectOfType<AudioManager>().Play("Dash");
		playerAimRay.ResetIsMovePossible();
		ClearWallBools();
		rb.velocity = transform.up * speed;
		aimRay.SetActive(false);
		isMoving = true;
		playerStats.playerShield.SetActive(false);
	}

	private void ClearWallBools()
	{
		onRightWall = false;
		onLeftWall = false;
		onTopWall = false;
		onBottomWall = false;
	}
}
