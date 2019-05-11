using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPickUpItems : MonoBehaviour {

    public GameObject promptButton;
    public GameObject playerAim;

    private GameObject promptButtonPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 promptButtonPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Instantiate(promptButton, promptButtonPosition, Quaternion.identity);
            promptButtonPrefab = promptButton;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playerAim.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(promptButtonPrefab);
        }
    }
}
