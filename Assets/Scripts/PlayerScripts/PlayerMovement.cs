using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("Scripts")]
	[SerializeField] private PlayerAimRayCast playerAimRayCast;
	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private PlayerAim playerAim;

	[Header("Particle effects")]
	[SerializeField] private ParticleSystem dashParticle;
	[SerializeField] private ParticleSystem jumpParticle;
	[SerializeField] private SpriteRenderer playerSprite;
	[SerializeField] private TrailRenderer playerTrail;


	[Header("Player stats")]
	[SerializeField] private float runSpeed;
	[SerializeField] private float dashSpeed;

	[Header("Misc")]
	[SerializeField] private Animator playerAnimator;
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private BoxCollider2D boxCollider;
	[SerializeField] private GameObject aimRay;
	[SerializeField] private Transform movementRaycast;
	[SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] private LayerMask groundLayerMask;

	[HideInInspector] public Vector3 lastTargetPosition;
	[HideInInspector] public Vector2 extents;
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
	private bool isGoingRight;
	private bool hasRunFlipped;
	private bool isTempRunSpeedSaved;
	private float horizontalMove;
	private float slowdownTimer;	
	private float jumpCooldown = 0.1f;
	private float turnAroundCooldown = 0.1f;
	private float jumpForce = 400f;
	private int footstepIndex;


	void Start ()
	{
		extents = boxCollider.bounds.extents;
		onTopWall = true;
		aimRay.SetActive(false);
		isGrounded = false;
	}

	void Update()
	{
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
					Dash();
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
					Dash();
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

		if (isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, playerAimRayCast.currentTargetPositionOffset, dashSpeed * Time.deltaTime);
			float distance = Vector2.Distance(transform.position, playerAimRayCast.currentTargetPositionOffset);
			if (distance <= 0.3f)
			{
				boxCollider.enabled = true;
			}
		}

		if (onTopWall && !isMoving)
		{
			cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.3f;
			rb.gravityScale = 3;
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			TurnAround();
			CheckDropForSlowdown();
					if (Input.GetButtonDown("Jump"))
        {
			jump = true;
		}
		}
		else
        {
			cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.0f;
			rb.gravityScale = 0;
        }
        jumpCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
	{
		if (onTopWall && !isMoving)
		{
			CheckGround();
			Run(horizontalMove * Time.fixedDeltaTime, jump);
			playerAnimator.SetFloat("RunSpeed", Mathf.Abs(horizontalMove));
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
		Vector2 boxSize = new Vector2(0.15f, 0.15f);
		float boxAngle = 0.0f;
		bool isGroundedBox = Physics2D.OverlapBox(movementRaycast.position, boxSize, boxAngle, groundLayerMask);
		if (isGroundedBox)
		{
			jumpCooldown = 0.1f;

			if (!isGrounded)
			{
				isGrounded = true;
				FindObjectOfType<AudioManager>().Play("Land");
				playerAnimator.SetBool("IsJumping", false);

			}
		}
		else
		{
			if (jumpCooldown <= 0.0f)
			{
				isGrounded = false;
				playerAnimator.SetBool("IsJumping", true);
			}
		}
	}

	private void CheckDropForSlowdown()
	{
		float rayDistance = 5.0f;
		RaycastHit2D hit = Physics2D.Raycast(movementRaycast.position, Vector2.down, rayDistance);
		if (hit.collider != null)
		{
			if (!hit.collider.CompareTag("Player"))
			{
				Time.timeScale = 1.0f;
				slowdownTimer = 0.3f;
			}
			else
			{
				slowdownTimer -= Time.deltaTime;
				if (slowdownTimer <= 0.0f)
				{
					Time.timeScale = 0.1f;
				}
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
			turnAroundCooldown -= Time.deltaTime;
			if (turnAroundCooldown <= 0)
			{
				turnAroundCooldown = 0.1f;
				isTempRunSpeedSaved = false;
			}
		}
		else
		{
			turnAroundCooldown = 0.1f;
		}

		if (isGoingRight && !hasRunFlipped)
		{
			if (horizontalMove < 0.0f)
			{
				playerAnimator.SetTrigger("RunFlip");
				hasRunFlipped = false;
				isGoingRight = false;
			}
		}
		else
		{
			if (horizontalMove > 0.0f)
			{
				playerAnimator.SetTrigger("RunFlip");
				hasRunFlipped = false;
				isGoingRight = true;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("RightWall"))
		{
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 90);

			onRightWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.enabled = false;
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("LeftWall"))
		{
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 270);

			onLeftWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.enabled = false;
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("TopWall"))
		{
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 0);

			hasRunFlipped = false;
			onTopWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.enabled = false;
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("BottomWall"))
		{
			Vector3 hit = other.contacts[0].normal;
			transform.rotation = Quaternion.Euler(0, 0, 180);

			onBottomWall = true;
			isMoving = false;
			onRotatable = true;
			playerTrail.enabled = false;
			dashParticle.Stop();
		}
		if (other.gameObject.CompareTag("Wall"))
		{
			transform.parent = other.gameObject.transform;
			Vector3 hit = other.contacts[0].normal;
			transform.up = hit;

			isMoving = false;
			onRotatable = true;
			playerTrail.enabled = false;
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

	private void Dash()
	{
		Time.timeScale = 1.0f;
		FindObjectOfType<AudioManager>().Play("Dash");
		playerAnimator.SetTrigger("Dash");
		playerAnimator.SetBool("IsJumping", false);
		playerAnimator.SetFloat("RunSpeed", Mathf.Abs(0));

		gameObject.transform.parent = null;
		boxCollider.enabled = false;
		hasRunFlipped = true;
		dashParticle.Play();
		playerTrail.enabled = true;
		isMoving = true;

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
