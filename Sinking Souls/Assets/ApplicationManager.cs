using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
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
        MAIN_MENU,
        TUTORIAL
    };

    public GameState state;

    public GameObject loadingScreen;
    [HideInInspector] public bool currentlyLoading = false;


    public TimelineAsset endGameCinematic;

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
        switch (sceneName)
        {
            case "Lobby":
                state = GameState.LOBBY;
                break;

            case "Game":
                state = GameState.GAME;
                break;

            case "Tavern":
                state = GameState.TABERN;
                break;

            case "MainMenu":
                state = GameState.MAIN_MENU;
                break;
            case "Tutorial":
                state = GameState.TUTORIAL;
                break;

            default:
                Debug.LogError("Doesn't exist a scene with name: " + sceneName);
                return;
        }

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
                sceneName = "Tavern";
                break;

            case GameState.MAIN_MENU:
                sceneName = "MainMenu";
                break;
            case GameState.TUTORIAL:
                sceneName = "Tutorial";
                break;
        }

        AudioManager.Instance.StopAll();
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName, float minLoadTime = 2.5f)
    {
        // Avoid loading multiple scenes at the same time.
        if (currentlyLoading) yield break;

        currentlyLoading = true;

        // Load scene and disable scene change until minLoadTime.
        var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        // Disable player movement.
        if (GameController.instance.player != null)
        {
            GameController.instance.player.GetComponent<Player>().Stop();
        }

        // Instantiate the loading screen on the canvas.
        Instantiate(loadingScreen, GameObject.Find("Canvas").transform);

        // Update the loading screen until isDone and minLoadTime.
        yield return new WaitForSecondsRealtime(minLoadTime);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        currentlyLoading = false;
    }

    public static void QuitApplication()
    {
        Application.Quit();
    }

    public void FinishGame()
    {
        CinematicManager.Instance.Play(endGameCinematic);
    }
}