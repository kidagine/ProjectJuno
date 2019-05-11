using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

    public Animator animatorFade;
    public Animator animatorCamera;
    public int indexOfSceneToLoad;

    public enum PositionForAnimation
    {
        Up,Down,Left,Right
    }
    readonly PositionForAnimation positionForAnimation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (positionForAnimation == PositionForAnimation.Up)
            { 
                StartCoroutine(SwitchScene());
                animatorCamera.SetTrigger("Up");
            }
            else if (positionForAnimation == PositionForAnimation.Down)
            {
                StartCoroutine(SwitchScene());
                animatorCamera.SetTrigger("Down");
            }
            else if (positionForAnimation == PositionForAnimation.Left)
            {
                StartCoroutine(SwitchScene());
                animatorCamera.SetTrigger("Left");
            }
            else if (positionForAnimation == PositionForAnimation.Right)
            {
                StartCoroutine(SwitchScene());
                animatorCamera.SetTrigger("Right");
            }
        }
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(indexOfSceneToLoad);
    }

}
