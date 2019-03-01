using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseContent;
    public GameObject resumeButton;

    public EventSystem eventSystem;

    private bool isPaused;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (InputManager.ButtonStart)
        {
            InputManager.ButtonStart = false;

            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (isPaused)
        {
            if (InputManager.ButtonB)
            {
                InputManager.ButtonB = false;
                Resume();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        pauseContent.SetActive(true);
        StartCoroutine(SelectButton());
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        isPaused = false;
        pauseContent.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // Needed to highlight the button when selecting it.
    private IEnumerator SelectButton()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        eventSystem.SetSelectedGameObject(resumeButton);
    }
}