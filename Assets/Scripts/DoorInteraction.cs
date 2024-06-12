using UnityEngine;
using UnityEngine.UI;

public class DoorInteraction : MonoBehaviour
{
    public GameObject contextClue;  // Assign the context clue UI element in the inspector
    public GameObject interactionPanel;  // Assign the panel UI element in the inspector
    private bool playerInRange = false;

    void Start()
    {
        contextClue.SetActive(false);
        interactionPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactionPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            contextClue.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            contextClue.SetActive(false);
            playerInRange = false;
        }
    }

    public void GetOut()
    {
        // Load the next scene, replace "NextScene" with your scene name
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test Scene Zach");
    }   

    public void Stay()
    {
        interactionPanel.SetActive(false);
    }
}
