using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public GameObject player;

    private LevelGenerator levelGenerator;
    private GameObject currentRoom;
    private Camera camera;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        levelGenerator = GetComponent<LevelGenerator>();
        camera = Camera.main;
        SpawnLevel();
    }

    private void SpawnLevel() {
        levelGenerator.Spawn();
    }

    private void SpawnPlayer() {

    }

    public void ChangeRoom(GameObject door) {

        Transform room = door.transform;
        while (room.parent != null) {
            room = room.parent.transform;
            if (room.parent.tag == "Room") break;
        }

    }

}
