using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDoor : MonoBehaviour {

    public string sceneToLoad;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            GameController.instance.scene = GameController.GameState.GAME;
            GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel++;
            Debug.Log(GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            switch (sceneToLoad) {
                case "Lobby":
                    GameController.instance.scene = GameController.GameState.LOBBY;

                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
                case "Game":
                    if(GameController.instance.scene == GameController.GameState.TABERN)
                    GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel--;

                    GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel++;
                    GameController.instance.scene = GameController.GameState.GAME;

                    if (GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel == 2 && !GameController.instance.gameObject.GetComponent<LevelGenerator>().tabernaSpawned)
                        GameController.instance.scene = GameController.GameState.TABERN;

                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
                case "LoadScene":
                    GameController.instance.scene = GameController.GameState.LOADSCENE;
                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
            }
        }
    }
}
