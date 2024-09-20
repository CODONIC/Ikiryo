using UnityEngine;
using UnityEngine.UI;

public class PanelTrigger : MonoBehaviour
{
    public GameObject contextClue;  // Assign the context clue UI element in the inspector
    public GameObject panel;  // Assign the panel UI element in the inspector
    private bool playerInRange = false;
    private bool panelActive = false;

    void Start()
    {
        contextClue.SetActive(false);
        panel.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!panelActive)
            {
                panel.SetActive(true);
                panelActive = true;
            }
            else
            {
                panel.SetActive(false);
                panelActive = false;
            }
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

            if (panelActive)
            {
                panel.SetActive(false);
                panelActive = false;
            }
        }
    }
}
