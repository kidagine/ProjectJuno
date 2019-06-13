using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public PlayerAimRayCast playerAimRayCast;
	public PlayerStats playerStats;
	public PlayerAim playerAim;
	public Rigidbody2D rb;
	public BoxCollider2D boxCollider;
	public GameObject aimRay;
	public GameObject movementRaycast;
	public GameObject playerTrail;
	public CinemachineVirtualCamera cinemachineVirtualCamera;
	public ParticleSystem dashParticle;
	public ParticleSystem jumpParticle;
	public SpriteRenderer playerSprite;
	public Animator animatorPlayer;
	public float runSpeed;
	public float dashSpeed;
	public Vector2 extents;

	[HideInInspector] public Vector3 lastTargetPosition;
	[HideInInspector] public bool isMoving;

	[HideInInspector] public bool onRightWall;
	[HideInInspector] public bool onLeftWall;
	[HideInInspector] public bool onTopWall;
	[HideInInspector] public bool onBottomWall;
	[HideInInspector] public bool onRotatable;


	private Vector3 rayPosition;
	private Vector3 rayPositionRight;
	private Vector3 rayPositionLeft;
	private bool facingRight;
    private bool jump;
    private bool isGrounded;
	private bool hasLanded;
	private float horizontalMove;
	private float slowdownTimer = 0.5f;	
	private float jumpCooldown = 0.1f;
    private float jumpForce = 400f;
	private readonly float rayDistance = 0.2f;
	private int footstepIndex;

	private float cooldownRunFlip = 0.1f;
	private bool isGoingRight;
	private bool hasRunFlipped;
	private bool isTempRunSpeedSaved;


	void Start ()
	{
		extents = boxCollider.bounds.extents;
		onTopWall = true;
		aimRay.SetActive(false);
	}

	void Update()
	{
		if (isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, playerAimRayCast.currentTargetPositionOffset, dashSpeed * Time.deltaTime);
			float distance = Vector2.Distance(transform.position, playerAimRayCast.currentTargetPositionOffset);
			if (distance <= 0.3f)
			{
				boxCollider.enabled = true;
			}
		}
		if (!Input.GetButtonDown("Fire1"))
		{
			//Using KB/M
			if (!PauseManager.isUsingController)
			{
				if (Input.GetMouseButton(1) && !isMoving)
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
				if (Input.GetAxis("CameraHorizontal") != 0 || Input.GetAxis("CameraVertical") != 0 && !isMoving)
				{
					aimRay.SetActive(true);
				}
				if (Input.GetButtonDown("Dash") && !isMoving && playerAimRayCast.IsMovePossible())
				{
					ResetMovement();
				}
				if (Input.GetAxis("CameraHorizontal") == 0 && Input.GetAxis("CameraVertical") == 0)
				{
					aimRay.SetActive(false);
				}
			}
		}
		else 
		{
			aimRay.SetActive(false);
		}


        if (Input.GetButtonDown("Jump"))
        {
            animatorPlayer.SetBool("IsJumping", true);
			jump = true;
		}
		if (rb.velocity.y != 0)
        {
            animatorPlayer.SetBool("IsJumping", true);
        }
        else
        {
            animatorPlayer.SetBool("IsJumping", false);
        }

        if (onTopWall && !isMoving)
		{
			cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.3f;
			rb.gravityScale = 3;
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			TurnAround();
		}
		else
        {
			cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.0f;
			rb.gravityScale = 0;
        }
        jumpCooldown -= Time.deltaTime;

		DetectLongFall();
    }

    void FixedUpdate()
	{
		if (onTopWall && !isMoving)
		{
			CheckGround();
			Run(horizontalMove * Time.fixedDeltaTime, jump);
			animatorPlayer.SetFloat("RunSpeed", Mathf.Abs(horizontalMove));
			jump = false;
		}
	}

	private void Run(float move, bool jump)
	{
		rb.velocity = new Vector2(move * 10, rb.velocity.y);
		if (move < 0)
		{
			playerSprite.flipX = true;
		}
		else if (move > 0)
		{
			playerSprite.flipX = false;
		}

		if (jump && isGrounded)
        {
			FindObjectOfType<AudioManager>().Play("Jump");
			rb.AddForce(new Vector2(0f, jumpForce));
			jumpParticle.Play();
			isGrounded = false;
		}
	}

	private void CheckGround()
	{
		RaycastHit2D hit = Physics2D.Raycast(movementRaycast.transform.position, Vector2.down, rayDistance);
		Debug.DrawRay(movementRaycast.transform.position, Vector2.down * rayDistance, Color.red);
		if (hit.collider != null)
		{
			if (!hit.collider.CompareTag("Player") && !jump)
			{
				isGrounded = true;
				jumpCooldown = 0.1f;
				if (!hasLanded)
				{
					FindObjectOfType<AudioManager>().Play("Land");
					hasLanded = true;
				}
			}
		}
		else
		{
			if (jumpCooldown <= 0)
			{
				isGrounded = false;
				hasLanded = false;
			}
		}
	}

	private void TurnAround()
	{
		if (horizontalMove != 0 && !isTempRunSpeedSaved)
		{
			if (horizontalMove > 0.0f)
			{
				isGoingRight = true;
			}
			else if (horizontalMove < 0.0f)
			{
				isGoingRight = false;
			}
			isTempRunSpeedSaved = true;
		}
		else if (horizontalMove == 0)
		{
			cooldownRunFlip -= Time.deltaTime;
			if (cooldownRunFlip <= 0)
			{
				cooldownRunFlip = 0.1f;
				isTempRunSpeedSaved = false;
			}
		}
		else
		{
			cooldownRunFlip = 0.1f;
		}

		if (isGoingRight && !hasRunFlipped)
		{
			if (horizontalMove < 0.0f)
			{
				animatorPlayer.SetTrigger("RunFlip");
				hasRunFlipped = false;
				isGoingRight = false;
			}
		}
		if (!isGoingRight && !hasRunFlipped)
		{
			if (horizontalMove > 0.0f)
			{
				animatorPlayer.SetTrigger("RunFlip");
				hasRunFlipped = false;
				isGoingRight = true;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("RightWall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 90);

			onRightWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("LeftWall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 270);

			onLeftWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("TopWall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 0);

			onTopWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("BottomWall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 180);

			onBottomWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("Wall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.up = hit;
			Debug.Log(hit);

			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
			dashParticle.Stop();
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
		gameObject.transform.parent = null;
		boxCollider.enabled = false;
		FindObjectOfType<AudioManager>().Play("Dash");
		animatorPlayer.SetTrigger("Dash");
		animatorPlayer.SetFloat("RunSpeed", Mathf.Abs(0));

		dashParticle.Play();
		playerTrail.SetActive(true);
		isMoving = true;

		transform.rotation = Quaternion.identity;
		horizontalMove = 0;
		rb.velocity = Vector2.zero;
		playerAimRayCast.ResetIsMovePossible();

		ClearWallBools();
		aimRay.SetActive(false);
		lastTargetPosition = playerAimRayCast.currentTargetPosition;
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

	private void DetectLongFall()
	{
		float slowdownTimerMax = 0.5f;
		if (!isGrounded && rb.gravityScale == 3)
		{
			slowdownTimer -= Time.deltaTime;
			if (slowdownTimer <= 0.0f)
			{
				Time.timeScale = 0.1f;
			}
		}
		else
		{
			if (Time.timeScale != 1.0f || slowdownTimer != slowdownTimerMax)
			{
				Time.timeScale = 1.0f;
				slowdownTimer = slowdownTimerMax;
			}
		}
	}

	public void PlayFootstepSound()
	{
		footstepIndex++;
		ArrayList listSounds = new ArrayList
		{
			FindObjectOfType<AudioManager>().GetSound("Footstep1"),
			FindObjectOfType<AudioManager>().GetSound("Footstep2"),
			FindObjectOfType<AudioManager>().GetSound("Footstep3"),
			FindObjectOfType<AudioManager>().GetSound("Footstep4"),
			FindObjectOfType<AudioManager>().GetSound("Footstep5"),
			FindObjectOfType<AudioManager>().GetSound("Footstep6")
		};
		if (footstepIndex >= listSounds.Count)
		{
			footstepIndex = 0;
		}
		Sounds s = (Sounds)listSounds[footstepIndex];
		s.source.Play();
	}

	public void PlaySound(string sound)
	{
		FindObjectOfType<AudioManager>().Play(sound);
	}

}
