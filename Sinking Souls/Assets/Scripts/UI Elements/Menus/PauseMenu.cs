using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseContent;
    public GameObject resumeButton;
    public GameObject settingsMenu;
    public Image backgroundImage;


    public bool isPaused;
    private bool inSettings;

    private void Start()
    {
        isPaused = false;
    }

    private void Update()
    {
        if (CinematicManager.Instance != null)
        {
            if (CinematicManager.Instance.isPlaying) return;
        }

        if (InputManager.ButtonStart)
        {
            InputManager.ButtonStart = false;

            if (!isPaused)
            {
                Pause();
                Cursor.visible = false;
                GameController.instance.cursor.GetComponent<mouseCursor>().InstaShow();
            }
            else
            {
                settingsMenu.SetActive(false);
                Resume();
                Cursor.visible = false;
                GameController.instance.cursor.GetComponent<mouseCursor>().InstaHide();
            }
        }

        if (!isPaused) return;

        if (!settingsMenu.activeSelf)
        {
            pauseContent.SetActive(true);

            if (InputManager.ButtonB)
            {
                InputManager.ButtonB = false;
                Resume();
            }
        }
    }

    public void HideCursor()
    {
        GameController.instance.cursor.GetComponent<mouseCursor>().InstaHide();
    }

    private void Pause()
    {
        isPaused = true;

        pauseContent.SetActive(true);

        var newColor = backgroundImage.color;
        newColor.a = 1.0f;
        backgroundImage.color = newColor;

        EventSystemWrapper.Instance.Select(resumeButton);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        isPaused = false;

        var newColor = backgroundImage.color;
        newColor.a = 0.0f;
        backgroundImage.color = newColor;

        pauseContent.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        ApplicationManager.QuitApplication();
    }

    public void ExitMenu()
    {
        Resume();
        ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.MAIN_MENU);
    }
}