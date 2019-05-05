using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRayCast : MonoBehaviour {

	public PlayerMovement playerMovement;
	public PlayerAimRayCastActive playerAimRayCastActive;
	public GameObject playerAimRayCastChild;
	public float distance;
	public bool isTouching;

	[HideInInspector] public Vector3 currentTargetPosition;
	[HideInInspector] public Vector3 lastTargetPosition;

	private LineRenderer lineRenderer;
	private Color activeAimColor;
	private Color disabledAimColor;
	private bool isMove;
	private bool isMovePossible;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	void Start ()
	{
		lineRenderer.useWorldSpace = true;
		ColorUtility.TryParseHtmlString("#ff006f", out activeAimColor);
		ColorUtility.TryParseHtmlString("#ffffff", out disabledAimColor);
	}

	void Update()
	{
		lineRenderer.SetPosition(0, gameObject.transform.position);
		Ray2D ray = new Ray2D(transform.position, transform.up);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distance);
		if (hit.collider != null)
		{
			if (hit.collider.CompareTag("IgnoreRaycast"))
			{
				lineRenderer.SetPosition(1, hit.point);
				lineRenderer.material.color = disabledAimColor;
				isTouching = false;
				if (isMovePossible)
					isMove = true;
				else
					isMove = false;
			}
			else
			{
				lineRenderer.SetPosition(1, hit.point);
				lineRenderer.material.color = activeAimColor;
				currentTargetPosition = hit.point;
				isTouching = true;
				isMove = true;
				isMovePossible = true;
			}
		}
		else
		{
			lineRenderer.SetPosition(1, ray.GetPoint(distance));
			lineRenderer.material.color = disabledAimColor;
			isTouching = false;
			if (isMovePossible)
			{
				isMove = true;
			}
			else
			{
				isMove = false;
			}
		}

		float distanceToChild = Vector3.Distance(gameObject.transform.position, playerAimRayCastChild.transform.position);

		if (isMovePossible)
		{
			isMove = true;
			playerAimRayCastChild.SetActive(true);
		}
		else
		{
			isMove = false;
			playerAimRayCastChild.SetActive(false);
		}

		if (playerMovement.isMoving)
			lastTargetPosition = currentTargetPosition;
		if (lastTargetPosition == currentTargetPosition)
			isMove = false;
	}

	public bool IsMovePossible()
	{
		return isMove;
	}

	public void ResetIsMovePossible()
	{
		isMove = false;
		IsMovePossible();
	}

	void OnEnable()
	{
		isMovePossible = false;
		StartCoroutine(SetLineRendererEnabled());
	}

	IEnumerator SetLineRendererEnabled()
	{
		lineRenderer.enabled = false;
		yield return new WaitForSeconds(0.02f);
		lineRenderer.enabled = true;
	}

}
