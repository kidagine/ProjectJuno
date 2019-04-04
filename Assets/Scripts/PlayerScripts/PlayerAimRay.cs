using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class PlayerAimRay : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public GameObject aimArrow;
	private Vector3 spawnHere;
	private Color activeAimColor;
	private bool isMove;

	void Start()
	{
		aimArrow.SetActive(false);
		isMove = false;
		ColorUtility.TryParseHtmlString("#ff006f", out activeAimColor);
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 4)
		{
			aimArrow.SetActive(true);
			spriteRenderer.color = activeAimColor;
			isMove = true;
			IsMovePossible();
			Debug.Log(other.name);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.layer == 4)
		{
			spriteRenderer.color = Color.white;
			isMove = false;
			IsMovePossible();
			Debug.Log(other.name);
		}
	}

	public bool IsMovePossible()
	{
		return isMove;
	}

	public void ResetIsMovePossible()
	{
		spriteRenderer.color = Color.white;
		isMove = false;
		IsMovePossible();
	}
}

