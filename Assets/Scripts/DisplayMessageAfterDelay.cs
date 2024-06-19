using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // Add this namespace for scene management

public class DisplayMessageAfterDelay : MonoBehaviour
{
    // Reference to the panel that will be displayed
    public GameObject panel;

    // Time in seconds before the panel is displayed
    public float delayTime = 180f; // Reduced delay for testing, can be adjusted

    // Time in seconds before transitioning to the next scene
    public float sceneTransitionDelay = 5f;

    void Start()
    {
        // Start the coroutine to wait and display the panel
        StartCoroutine(ShowPanelAfterDelay());
    }

    private IEnumerator ShowPanelAfterDelay()
    {
        // Wait for the specified delay time (3 minutes = 180 seconds)
        yield return new WaitForSeconds(delayTime);

        // Activate the panel
        panel.SetActive(true);

        // Wait for a brief moment before transitioning to the next scene
        yield return new WaitForSeconds(sceneTransitionDelay);

        // Load the next scene
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        // You can load the next scene by scene name or index
        // Example by scene name:
        SceneManager.LoadScene("StarterRoom");

        // Example by scene index:
        // SceneManager.LoadScene(1); // Replace 1 with the index of the next scene
    }
}
