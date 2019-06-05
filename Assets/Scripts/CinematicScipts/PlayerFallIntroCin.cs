using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallIntroCin : MonoBehaviour
{

	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private GameObject beam;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private PlayerAim playerAim;

	void Start()
    {
		DisablePlayer();
	}

    void Update()
    {
		rb.gravityScale = 3;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Invoke("EnablePlayer",1);
		Destroy(GetComponent<PlayerFallIntroCin>(),1);
	}

	private void EnablePlayer()
	{
		beam.SetActive(true);
		playerMovement.enabled = true;
		playerAim.enabled = true;
	}

	private void DisablePlayer()
	{
		beam.SetActive(false);
		playerMovement.enabled = false;
		playerAim.enabled = false;
	}

}
