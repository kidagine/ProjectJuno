using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public GameObject playerShield;
	public Rigidbody2D playerRigidbody;
	public Slider currentHealthSlider;
	public Text currentHealthTxt;
	public CameraShaker cameraShaker;
	public PixelBoy pixelBoy;
	public Animator animator;
	public int health = 3;


	private PlayerMovement playerMovement;
	private float vulnerabilityCooldown = 0.3f;
	//private PixelBoy pixelBoy;

	void Start()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement>();
		//pixelBoy = gameObject.GetComponent<PixelBoy>();
		currentHealthSlider.maxValue = health;
	}

	void Update()
	{
		vulnerabilityCooldown -= Time.deltaTime;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Deadly"))
		{
			Vector2 dir = (transform.position - other.transform.position).normalized;
			playerRigidbody.AddForce(dir * 200);
			StartCoroutine(ResetVelocity());
			TakeDamage(1);
			playerShield.SetActive(true); 
		}
	}

	public void TakeDamage(int damage)
	{
		if (vulnerabilityCooldown <= 0)
		{
			FindObjectOfType<AudioManager>().Play("Hit");
			cameraShaker.CameraShake();
			StartCoroutine(ResetVelocity());

			health = health - damage;
			currentHealthSlider.value = health;
			currentHealthTxt.text = health + "";
			vulnerabilityCooldown = 0.45f;
			animator.SetTrigger("Hit");
		}
	}

	IEnumerator ResetVelocity()
	{
		pixelBoy.DecreaseResolution(150f);
		yield return new WaitForSeconds(0.225f);
		playerRigidbody.velocity = Vector2.zero;
		playerMovement.isMoving = false;
	}

}
