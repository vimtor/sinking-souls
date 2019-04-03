using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenuContent;
    public GameObject resumeButton;

    private void Start()
    {
        if (!SaveManager.CheckFile())
        {
            Destroy(resumeButton);
        }
    }

    private void Update()
    {
        if (!settingsMenu.activeSelf)
        {
            mainMenuContent.SetActive(true);
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
