using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimRayCast : MonoBehaviour {

	public GameObject arrowAim;
	public float distance;

	private LineRenderer lineRenderer;
	private Color activeAimColor;
	private Color disabledAimColor;
	private bool isMove;

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
			if (hit.collider.CompareTag("Deadly"))
			{
				arrowAim.SetActive(false);
				lineRenderer.SetPosition(1, hit.point);
				lineRenderer.material.color = disabledAimColor;
				isMove = false;
			}
			else
			{
				SetArrowRotation(hit);
				arrowAim.SetActive(true);
				arrowAim.transform.position = hit.point;
				lineRenderer.SetPosition(1, hit.point);
				lineRenderer.material.color = activeAimColor;
				isMove = true;
			}
		}
		else
		{
			arrowAim.SetActive(false);
			lineRenderer.SetPosition(1, ray.GetPoint(distance));
			lineRenderer.material.color = disabledAimColor;
			isMove = false;
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

	public bool IsMovePossible()
	{
		return isMove;
	}

	public void ResetIsMovePossible()
	{
		isMove = false;
		IsMovePossible();
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
		yield return new WaitForSeconds(0.02f);
		lineRenderer.enabled = true;
	}

}
