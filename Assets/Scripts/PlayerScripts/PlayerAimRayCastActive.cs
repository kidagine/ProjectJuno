using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRayCastActive : MonoBehaviour {

	public PlayerMovement playerMovement;
	public PlayerAimRayCast playerAimRayCast;
	public GameObject playerAimRayCastObject;
	public GameObject arrowAim;
	public float distance;

	[HideInInspector] public Vector3 currentTargetPosition;
	[HideInInspector] public Vector3 lastTargetPosition;

	private LineRenderer lineRenderer;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Start()
	{
		lineRenderer.useWorldSpace = true;
	}

	void FixedUpdate()
	{
		lineRenderer.SetPosition(0, gameObject.transform.position);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distance);
		if (hit.collider != null)
		{
				SetArrowRotation(hit);
				arrowAim.SetActive(true);
				arrowAim.transform.position = playerAimRayCast.currentTargetPosition;
				arrowAim.transform.up = hit.normal;
				lineRenderer.SetPosition(1, playerAimRayCast.currentTargetPosition);
				currentTargetPosition = hit.point;

		}
		if (!playerAimRayCast.isTouching)
		{
			gameObject.transform.parent = null;
		}
		else
		{
			ResetPosition();
		}

		if (playerMovement.isMoving)
		{
			gameObject.SetActive(false);
		}

	}

	private void SetArrowRotation(RaycastHit2D hit)
	{
		if (hit.collider.CompareTag("LeftWall"))
		{
			arrowAim.transform.eulerAngles = new Vector3(0, 0, 90);
		}
		if (hit.collider.CompareTag("RightWall"))
		{
			arrowAim.transform.eulerAngles = new Vector3(0, 0, 270);
		}
		if (hit.collider.CompareTag("TopWall"))
		{
			arrowAim.transform.eulerAngles = new Vector3(0, 0, 180);
		}
		if (hit.collider.CompareTag("BottomWall"))
		{
			arrowAim.transform.eulerAngles = new Vector3(0, 0, 0);
		}
	}

	public void ResetPosition()
	{
		gameObject.transform.parent = playerAimRayCastObject.transform;
		gameObject.transform.position = playerAimRayCastObject.transform.position;
		gameObject.transform.rotation = playerAimRayCastObject.transform.rotation;
	}

	void OnDisable()
	{
		arrowAim.SetActive(false);
	}

	void OnEnable()
	{
		StartCoroutine(SetLineRendererEnabled());
	}

	IEnumerator SetLineRendererEnabled()
	{
		lineRenderer.enabled = false;
		yield return new WaitForSeconds(0.04f);
		lineRenderer.enabled = true;
	}

}
