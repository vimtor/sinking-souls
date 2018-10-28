using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobyDoor : MonoBehaviour {

    public string sceneToLoad;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            switch (sceneToLoad) {
                case "Lobby":
                    GameController.instance.scene = GameController.GameState.LOBBY;
                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
                case "Game":
                    GameController.instance.scene = GameController.GameState.GAME;
                    SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
                    break;
            }
        }
    }
}
