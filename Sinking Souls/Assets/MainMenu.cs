using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenuContent;
    public GameObject resumeButton;

    private bool hiding;

    private void Start()
    {
        if (!SaveManager.CheckFile())
        {
            Destroy(resumeButton);
        }

        AudioManager.Instance.PlayMusic("TitleScreen");
    }

    private void Update()
    {
        if (!settingsMenu.activeSelf)
        {
            mainMenuContent.SetActive(true);
        }

        UpdateMouse();
    }

    // Hide cursor if not use it for certain time.
    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSeconds(time);
        if (hiding) Cursor.visible = false;
    }

    private void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            Cursor.visible = true;
        }
        else if (!hiding)
        {
            hiding = true;
            StartCoroutine(HideMouse(3.0f));
        }
    }

    public void Play()
    {
        GameController.instance.LoadGame();
        AudioManager.Instance.PlayMusic("Soundtrack");

        ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.LOBBY);
    }

    public void NewGame()
    {
        GameController.instance.lobbySouls = 0;
        GameController.instance.m_RescuedAlchemist = false;
        GameController.instance.m_RescuedBlacksmith = false;

        ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.TUTORIAL);
    }

    public void Exit()
    {
        ApplicationManager.QuitApplication();
    }
}
