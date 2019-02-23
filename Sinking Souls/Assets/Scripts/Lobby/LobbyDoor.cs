using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDoor : MonoBehaviour {

    public string sceneToLoad;
    private Vector3 startPosition;

    private void Start() {
        startPosition = transform.position;
    }

    private void Update() {
        if (GameObject.Find("Ailin_Boss(Clone)") != null) transform.position = startPosition + Vector3.down * 5;
        else transform.position = startPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (sceneToLoad)
        {
            case "Lobby":
                GameController.instance.ChangeScene(sceneToLoad);
                break;

            case "Game":
                if (GameController.instance.scene == GameController.GameState.TABERN)
                {
                    GameController.instance.LevelGenerator.currentLevel--;
                }
                

                GameController.instance.LevelGenerator.currentLevel++;
                if (GameController.instance.LevelGenerator.currentLevel == 2 && !GameController.instance.LevelGenerator.tabernaSpawned)
                {
                    // CHANGE THIS TO UNIQUE TAVERN SCENE.
                    GameController.instance.scene = GameController.GameState.TABERN;
                    SceneManager.LoadScene("Game", LoadSceneMode.Single);
                    return;
                }

                GameController.instance.ChangeScene("Game");
                break;

            case "LoadScene":
                GameController.instance.ChangeScene("LoadScene");
                break;
        }
    }
}
