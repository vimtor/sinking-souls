using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobbyDoor : MonoBehaviour {

    public string sceneToLoad;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            switch (sceneToLoad) {
                case "Lobby":
                    GameController.instance.scene = GameController.GameState.LOBBY;
                    foreach(Modifier modifier in GameController.instance.pickedModifiers) {
                        modifier.unlocked = true;
                    }

                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
                case "Game":
                    GameController.instance.scene = GameController.GameState.GAME;
                    GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel++;
                    Debug.Log(GameController.instance.gameObject.GetComponent<LevelGenerator>().currentLevel);
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
