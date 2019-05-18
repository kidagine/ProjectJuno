using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

    public Animator animatorFade;
    public Animator animatorCamera;
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
        if (other.gameObject.CompareTag("Player"))
        {
			rb = other.GetComponent<Rigidbody2D>();
			FindObjectOfType<PauseManager>().DisablePlayerMovement();

			Vector3 test = new Vector3(-2, -8, 0);
			SceneTransitionManager.Instance.SetPlayerPosition(test);

			animatorFade.SetTrigger("FadeOut");
			StartCoroutine(SwitchScene());

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

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene(indexOfSceneToLoad);

	}

}
