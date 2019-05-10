using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggerIntroStageOne : MonoBehaviour {

	public GameObject player;
	public Animator UIPlayerAnim;
	public Animator UIStageTitleAnim;
	public Animator cinemachineAnim;
	public AudioSource backgroundMusic;

	private Rigidbody2D playerRigidbody;
	private int speed = -20;

	void Start()
	{
		FindObjectOfType<AudioManager>().Play("FadeOut");
		cinemachineAnim.SetTrigger("ZoomIn");
		backgroundMusic.Pause();
		playerRigidbody = player.GetComponent<Rigidbody2D>();
		playerRigidbody.velocity = transform.up * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			FindObjectOfType<AudioManager>().Play("Impact");
			backgroundMusic.Play();
			UIPlayerAnim.SetTrigger("FadeIn");
			UIStageTitleAnim.SetTrigger("FadeIn");
			Destroy(gameObject);
		}
	}
}
