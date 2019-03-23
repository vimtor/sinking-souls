using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTraining : MonoBehaviour {
    public int times;
    private int counter = 0;
    bool stop = false;

	void Update () {
        if (!stop) {
            if (GameController.instance.player.GetComponent<Player>().lockedEnemy == gameObject) {
                if (GameController.instance.player.GetComponent<Player>().lockedDashed) {
                    GameController.instance.player.GetComponent<Player>().lockedDashed = false;
                    counter++;
                }
            }
            if (counter == times) {
                GameController.instance.roomEnemies = new List<GameObject>();
                times = 0;
                stop = true;
            }
        }
	}
}
