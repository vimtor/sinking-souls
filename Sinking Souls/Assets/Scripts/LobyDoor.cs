using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LobyDoor : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameController.instance.scene = GameController.GameState.GAME;
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }
}
