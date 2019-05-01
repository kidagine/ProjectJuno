using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {

	public PlayerAimRayCast playerAimRayCast;
	public PlayerStats playerStats;
	public PlayerAim playerAim;
	public Rigidbody2D rb;
	public GameObject aimRay;
	public GameObject playerAimPoint;
	public GameObject dashEffectPrefab;
	public Animator animatorPlayer;

	[HideInInspector] public bool isMoving;
	[HideInInspector] public int speed;

	[HideInInspector] public bool onRightWall;
	[HideInInspector] public bool onLeftWall;
	[HideInInspector] public bool onTopWall;
	[HideInInspector] public bool onBottomWall;
	[HideInInspector] public bool onRotatable;


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
				if (Input.GetMouseButtonUp(1) && !isMoving && playerAimRayCast.IsMovePossible() && playerAim.isMovePossible)
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
				if (Input.GetButtonDown("Dash") && !isMoving && playerAimRayCast.IsMovePossible())
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
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 90);
			rb.velocity = Vector2.zero;
			isMoving = false;
			onRightWall = true;
		}
		if (other.gameObject.CompareTag("LeftWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 270);
			rb.velocity = Vector2.zero;
			isMoving = false;
			onLeftWall = true;
		}
		if (other.gameObject.CompareTag("TopWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 0);
			rb.velocity = Vector2.zero;
			isMoving = false;
			onTopWall = true;
		}
		if (other.gameObject.CompareTag("BottomWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 180);
			rb.velocity = Vector2.zero;
			isMoving = false;
			onBottomWall = true;
		}
		if (other.gameObject.CompareTag("Wall"))
		{
			transform.parent = other.gameObject.transform;
			rb.velocity = Vector2.zero;
			isMoving = false;
			onRotatable = true;
		}

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("KillLine"))
		{
			playerStats.Dead();
		}
	}

	private void ResetMovement()
	{
		FindObjectOfType<AudioManager>().Play("Dash");
		animatorPlayer.SetTrigger("Dash");
		gameObject.transform.parent = null;
		Instantiate(dashEffectPrefab, transform.position, transform.rotation);
		playerAimRayCast.ResetIsMovePossible();
		ClearWallBools();
		rb.velocity = playerAimPoint.transform.up * speed;
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
		onRotatable = false;
	}
}
