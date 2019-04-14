using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
	public Rigidbody2D playerRigidbody;
	public Slider currentHealthSlider;
	public Text currentHealthTxt;
	public CameraShaker cameraShaker;
	public PixelBoy pixelBoy;
	public int health = 3;

	private PlayerMovement playerMovement;
	//private PixelBoy pixelBoy;

	void Start()
	{
		playerMovement = gameObject.GetComponent<PlayerMovement>();
		//pixelBoy = gameObject.GetComponent<PixelBoy>();
		currentHealthSlider.maxValue = health;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Deadly"))
		{
			Vector2 knockBackDir = (other.transform.position - transform.position).normalized;
			knockBackDir.y = 0;
			Debug.Log(knockBackDir);
			playerRigidbody.AddForce(new Vector2(-200,0));
			StartCoroutine(ResetVelocity());
			cameraShaker.CameraShake();
			FindObjectOfType<AudioManager>().Play("Hit");

			health--;
			currentHealthSlider.value = health;
			currentHealthTxt.text = health + "";
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
