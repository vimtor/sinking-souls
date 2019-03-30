using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject mainMenuContent;

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

    public void Exit()
    {
        ApplicationManager.QuitApplication();
    }
}
