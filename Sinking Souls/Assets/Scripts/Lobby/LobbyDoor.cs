using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDoor : MonoBehaviour {

    public string sceneToLoad;

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
