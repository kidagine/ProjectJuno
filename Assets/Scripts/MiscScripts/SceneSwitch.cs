using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

    public Animator animatorFade;
    public Animator animatorCamera;
	public SpriteRenderer playerSpriteRenderer;
    public int indexOfSceneToLoad;

	private Rigidbody2D rb;
	private float playerMove = 2f;

    public enum PositionForAnimation
    {
        Up,Down,Left,Right
    }
    readonly PositionForAnimation positionForAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
		bool isFacingRight;
		if (other.gameObject.CompareTag("Player"))
        {
			Vector3 test = new Vector3(-2, -8, 0);
			isFacingRight = playerSpriteRenderer.flipX;
			SceneTransitionManager.Instance.SetPlayerPosition(test);
			SceneTransitionManager.Instance.SetPlayerFacing(isFacingRight);
			SceneManager.LoadScene(indexOfSceneToLoad);

			//if (positionForAnimation == PositionForAnimation.Up)
   //         { 
   //             animatorCamera.SetTrigger("Up");
   //         }
   //         else if (positionForAnimation == PositionForAnimation.Down)
   //         {
   //             StartCoroutine(SwitchScene());
   //             animatorCamera.SetTrigger("Down");
   //         }
   //         else if (positionForAnimation == PositionForAnimation.Left)
   //         {
   //             StartCoroutine(SwitchScene());
   //             animatorCamera.SetTrigger("Left");
   //         }
   //         else if (positionForAnimation == PositionForAnimation.Right)
   //         {
   //             StartCoroutine(SwitchScene());
   //             animatorCamera.SetTrigger("Right");
   //         }
        }
    }

	private void OnTriggerStay2D(Collider2D other)
	{
		if (rb != null && other.gameObject.CompareTag("Player"))
		{
			rb.velocity = new Vector2(playerMove * -1, rb.velocity.y);
		}
	}

}
