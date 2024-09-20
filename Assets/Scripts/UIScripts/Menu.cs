using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject menuPanel;
    private bool isPaused = false;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menuPanel.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    void OpenMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
        isPaused = true;
    }

    public void CloseMenu()
    {
        if (menuPanel.activeSelf)
        {
            menuPanel.SetActive(false);
            Time.timeScale = 1f; // Resume the game
            isPaused = false;
        }
    }

    public void GoToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
