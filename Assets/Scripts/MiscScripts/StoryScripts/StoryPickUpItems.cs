using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryPickUpItems : MonoBehaviour {

	public GameObject itemToPickUp;
    public GameObject promptButton;
    public GameObject playerAim;
	public GameObject itemMenu;
	public GameObject itemMenuImage;
	public Animator itemMenuAnimator;
	public Text itemNameText;
	public Text itemDescriptionText;
	public string itemName;
	[TextArea] public string itemDescription;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			if (!playerAim.activeSelf)
			{
				Vector3 promptButtonPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
				promptButton.transform.position = promptButtonPosition;
				promptButton.SetActive(true);
			}
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			if (!itemMenu.activeSelf)
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					itemMenu.SetActive(true);
					itemMenuImage.SetActive(true);
					playerAim.SetActive(true);
					promptButton.SetActive(false);
					SceneTransitionManager.Instance.HasPickedItem(true);
					itemToPickUp.SetActive(true);
					//TODO I dont have animation yet. playerAnimation.SetBool("PickUpItem",true);
					FindObjectOfType<PauseManager>().DisablePlayerMovement();
					itemMenuAnimator.SetTrigger("Open");
					itemNameText.text = itemName;
					itemDescriptionText.text = itemDescription;
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					FindObjectOfType<PauseManager>().EnablePlayerMovement();
					Vector2 itemPickParticlesPosition = itemToPickUp.transform.position;
					Destroy(itemToPickUp);
					//TODO I dont have animation yet. playerAnimation.SetBool("PickUpItem",false);
					itemMenuAnimator.SetTrigger("Close");
					itemMenuImage.SetActive(false);
					itemNameText.text = "";
					itemDescriptionText.text = "";
				}
			}
		}
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			promptButton.SetActive(false);
		}
	}
}
