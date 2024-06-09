using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public float fadeDuration = 2f;
    public TextMeshProUGUI textMesh;

    private bool fadingIn = true;
    private float fadeStartTime;

    void Start()
    {
        // Start the fade-in effect
        FadeIn();
    }

    void Update()
    {
        if (fadingIn)
        {
            // Calculate time elapsed since the start of fade-in
            float timeElapsed = Time.time - fadeStartTime;
            // Gradually increase alpha value for fade-in effect
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration));
            if (timeElapsed >= fadeDuration)
            {
                // Switch to fade-out when fade-in is complete
                fadingIn = false;
                fadeStartTime = Time.time;
            }
        }
        else
        {
            // Calculate time elapsed since the start of fade-out
            float timeElapsed = Time.time - fadeStartTime;
            // Gradually decrease alpha value for fade-out effect
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration));
            if (timeElapsed >= fadeDuration)
            {
                // Restart fade-in when fade-out is complete
                fadingIn = true;
                FadeIn(); // Restart fade-in effect
            }
        }
    }

    void FadeIn()
    {
        // Reset alpha value for fade-in
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0f);
        // Record start time of fade-in
        fadeStartTime = Time.time;
    }

    // This method is called when the user taps on the screen
    public void OnScreenTapped()
    {
        LoadNextLevel();
    }

    // Method to load a specific scene based on player choice
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadNextLevel()
    {
        
            StartCoroutine(SceneTransition(1)); // Load scene with build index 1
        

    }

    IEnumerator SceneTransition(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}
