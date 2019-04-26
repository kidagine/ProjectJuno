using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public GameObject playerShield;
	public GameObject playerDeathPrefab;
	public Rigidbody2D playerRigidbody;
	public Slider currentHealthSlider;
	public Slider expSlider;
	public Text currentHealthTxt;
	public Text receivedExpText;
	public Text levelText;
	public CameraShaker cameraShaker;
	public PixelBoy pixelBoy;
	public Animator animatorPlayer;
	public Animator animatorExpReceivedText;
	public int health = 3;


	private PlayerMovement playerMovement;
	private float vulnerabilityCooldown = 0.3f;
	private int level = 2;
	private int currentExpTotal;
	private bool hasReceivedExp;
	private bool isExpSliderIncreasing;

	void Start()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement>();
		currentHealthSlider.maxValue = health;
		level = PlayerPrefs.GetInt("level", level);
		levelText.text = level + "";
	}

	void Update()
	{
		if (hasReceivedExp)
		{
			showExpReceived();
		}
		if (isExpSliderIncreasing)
		{
			graduallyIncreaseExp();
		}
		vulnerabilityCooldown -= Time.deltaTime;
	}

	//HEALTH
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
				animatorPlayer.SetTrigger("Hit");
		}
		if (health <= 0)
		{
			Dead(); 
		}
	}

	IEnumerator ResetVelocity()
	{
		yield return new WaitForSeconds(0.225f);
		playerRigidbody.velocity = Vector2.zero;
		playerMovement.isMoving = false;
	}

	public void Dead()
	{
		FindObjectOfType<AudioManager>().Play("Death");
		Instantiate(playerDeathPrefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
		pixelBoy.DecreaseResolution(3);
		PlayerPrefs.SetInt("level", level);
		Destroy(gameObject);
	}

	//LEVEL
	public void receiveExp(int enemyExp)
	{
		currentExpTotal += enemyExp;
		if (expSlider.value != enemyExp)
		{
			isExpSliderIncreasing = true;
		}
		receivedExpText.text = enemyExp + " exp";
		hasReceivedExp = true;
	}

	private void showExpReceived()
	{
		Vector2 receivedExpTextPosition = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x+0.7f,transform.position.y+0.5f));
	
		receivedExpText.transform.position = receivedExpTextPosition;
		animatorExpReceivedText.SetTrigger("Show");
		hasReceivedExp = false;
	}

	private void graduallyIncreaseExp()
	{
		expSlider.value += 0.1f;
		if (expSlider.value >= currentExpTotal)
			isExpSliderIncreasing = false;
		if (expSlider.value == expSlider.maxValue)
		{
			level++;
			levelText.text = level + "";
			expSlider.value = 0;
			currentExpTotal = 0;
		}
	}

}
