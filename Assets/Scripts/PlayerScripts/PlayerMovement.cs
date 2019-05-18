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
	public GameObject dashEffectPrefab;
	public GameObject movementRaycast;
	public GameObject playerTrail;
	public SpriteRenderer playerSprite;
	public Animator animatorPlayer;
	public float runSpeed;

	[HideInInspector] public Vector3 lastTargetPosition;
	[HideInInspector] public bool isMoving;
	[HideInInspector] public int speed;

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
	private float horizontalMove;
    private float jumpCooldown = 0.1f;
    private float jumpForce = 400f;
	private readonly float rayDistance = 0.2f;


	void Start ()
	{
		onTopWall = true;
		aimRay.SetActive(false);
	}

	void Update()
	{
		if (isMoving)
		{
			transform.position = Vector3.MoveTowards(transform.position, playerAimRayCast.currentTargetPosition, speed * Time.deltaTime);
			Debug.Log("MOVING");
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
            FindObjectOfType<AudioManager>().Play("Jump");
            jump = true;
            animatorPlayer.SetBool("IsJumping", true);
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
            rb.gravityScale = 3;
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		}
		else
        {
            rb.gravityScale = 0;
        }
        jumpCooldown -= Time.deltaTime;
    }

    void FixedUpdate()
	{
		if (onTopWall && !isMoving)
		{
			CheckGround();
			playerSprite.flipX = false;
			Run(horizontalMove * Time.fixedDeltaTime, jump);
			animatorPlayer.SetFloat("RunSpeed", Mathf.Abs(horizontalMove));
			jump = false;
		}
	}

	private void Run(float move, bool jump)
	{
		rb.velocity = new Vector2(move * 10, rb.velocity.y);
		if (move < 0 && !facingRight)
		{
			Flip();
		}
		else if (move > 0 && facingRight)
		{
			Flip();
		}

        if (jump && isGrounded)
        {
			Debug.Log("TEST");
            rb.AddForce(new Vector2(0f, jumpForce));
			isGrounded = false;
		}
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
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
			}
		}
		else
		{
			{
				if (jumpCooldown <= 0)
				{
					isGrounded = false;
				}
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("RightWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 90);
			isMoving = false;
			onRightWall = true;
			playerTrail.SetActive(false);
		}
		if (other.gameObject.CompareTag("LeftWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 270);
			isMoving = false;
			onLeftWall = true;
			playerTrail.SetActive(false);
		}
		if (other.gameObject.CompareTag("TopWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 0);
			isMoving = false;
			onTopWall = true;
			playerTrail.SetActive(false);
		}
		if (other.gameObject.CompareTag("BottomWall"))
		{
			Instantiate(dashEffectPrefab, transform.position, transform.rotation);
			transform.rotation = Quaternion.Euler(0, 0, 180);
			isMoving = false;
			onBottomWall = true;
			playerTrail.SetActive(false);
		}
		if (other.gameObject.CompareTag("Wall"))
		{
			transform.parent = other.gameObject.transform;
			isMoving = false;
			onRotatable = true;
			playerTrail.SetActive(false);
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
		animatorPlayer.SetFloat("RunSpeed", Mathf.Abs(0));

        playerTrail.SetActive(true);
		isMoving = true;

		horizontalMove = 0;
		rb.velocity = Vector2.zero;
		Instantiate(dashEffectPrefab, transform.position, transform.rotation);
		playerAimRayCast.ResetIsMovePossible();
		if (onRotatable)
		{
			gameObject.transform.parent = null;
		}
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
}
