using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBreakChallenge : Challenge {

    public List<GameObject> amphs = new List<GameObject>();
    int count = 0;

    public override void AlwaysUpdate() {
    }

    public override void Initialize() {

    }

    public override void LevelStart() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).gameObject.GetComponent<DestructibleProp>()) {
                amphs.Add(transform.GetChild(i).gameObject);
                if (transform.GetChild(i).gameObject.activeSelf) count++;
            }
        }
        int randVal = Random.Range(8, 13);
        foreach (GameObject g in amphs) {
            if (count >= randVal) break;
            if (!g.activeSelf) {
                g.SetActive(true);
                count++;
            }
        }
    }
    public override void Loose() {

        Debug.Log("LOOSE");
    }
    bool checkNull() {
        for (int i = 0; i < amphs.Count; i++) {
            if (amphs[i] == null) return true;

        }
        return false;
    }

    public override ChallengeState StartedUpdate() {
        if (EnemiesAlive()) {
            if (checkNull()) return newState(false, false);
            else return newState(true, false);

        }
        else {
            return newState(false, true);
        }
    }

    public override void Win() {
        Debug.Log("WIIIIN");

    }

}

