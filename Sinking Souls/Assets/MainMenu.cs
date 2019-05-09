using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenuContent;
    public GameObject resumeButton;

    private bool hiding;
    private bool hideForced;

    private void Start()
    {
        if (!SaveManager.CheckFile())
        {
            Destroy(resumeButton);
        }
        hideForced = false;

        AudioManager.Instance.PlayMusic("TitleScreen");
    }

    private void Update()
    {

        if (!settingsMenu.activeSelf)
        {
            mainMenuContent.SetActive(true);
        }

        if(!hideForced) UpdateMouse();
    }

    // Hide cursor if not use it for certain time.
    private IEnumerator HideMouse(float time)
    {
        yield return new WaitForSeconds(time);
        if (hiding)
        {
            Cursor.visible = false;
            GameController.instance.cursor.GetComponent<mouseCursor>().Hide();

        }
    }

    private void UpdateMouse()
    {
        if (Math.Abs(InputManager.Mouse.magnitude) > 0.0f)
        {
            hiding = false;
            //Cursor.visible = true;
            GameController.instance.cursor.GetComponent<mouseCursor>().Show();

        }
        else if (!hiding && GetComponent<ButtonsController>().hit.collider == null)
        {
            hiding = true;
            StartCoroutine(HideMouse(3.0f));
        }
    }

    public void ForceHide()
    {
        hideForced = true;
        Cursor.visible = false;
        GameController.instance.cursor.GetComponent<mouseCursor>().InstaHide();

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
