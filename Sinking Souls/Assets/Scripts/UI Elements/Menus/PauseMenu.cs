using System.Collections;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseContent;
    public GameObject resumeButton;

    private bool _isPaused;

    private void Start()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if (InputManager.ButtonStart)
        {
            InputManager.ButtonStart = false;

            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        if (_isPaused)
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
        _isPaused = true;
        pauseContent.SetActive(true);
        EventSystemWrapper.Instance.SelectFirst(resumeButton);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        _isPaused = false;
        pauseContent.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        GameController.instance.QuitApplication();
    }

    public void ExitMenu()
    {
        GameController.instance.ChangeScene("MainMenu");
    }
}