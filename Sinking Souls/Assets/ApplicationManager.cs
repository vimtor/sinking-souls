using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApplicationManager : MonoBehaviour
{
    public enum GameState
    {
        LOBBY,
        GAME,
        ARENA,
        LOADSCENE,
        TABERN,
        MAIN_MENU
    };

    public GameState state;

    public GameObject loadingScreen;

    public static ApplicationManager Instance { get; private set; }

    private void Awake()
    {
        #region SINGLETON

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        #endregion
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameController.instance.SetupScene(state);
    }

    public void ChangeScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void ChangeScene(GameState newState)
    {
        var sceneName = "";
        state = newState;

        switch (state)
        {
            case GameState.LOBBY:
                sceneName = "Lobby";
                break;

            case GameState.GAME:
                sceneName = "Game";
                break;

            case GameState.ARENA:
                sceneName = "Arena";
                break;

            case GameState.LOADSCENE:
                sceneName = "LoadScene";
                break;

            case GameState.TABERN:
                sceneName = "Game";
                break;

            case GameState.MAIN_MENU:
                sceneName = "MainMenu";
                break;
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // Instantiate the loading screen on the canvas.
        Instantiate(loadingScreen, GameObject.Find("Canvas").transform);
        var loadingBar = GameObject.Find("Loading Bar").GetComponent<Image>();


        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / .9f);
            loadingBar.fillAmount = progress;

            yield return null;
        }
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}