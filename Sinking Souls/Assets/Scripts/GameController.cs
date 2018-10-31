using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public enum GameState { LOBBY, GAME };
    [HideInInspector] public GameState scene = GameState.LOBBY;
    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public GameObject playerGO;
    [HideInInspector] public GameObject player;
    public bool blacksmith = false; /// Consider making this a array that holds the unlocked/locked state of each friend
    public GameObject blueprint;
    public List<Modifier> modifiers;
    public List<Modifier> pickedModifiers;
    public int blueSouls;
    public int redSouls;
    public int greenSouls;

    private LevelGenerator levelGenerator;

    private void Awake() {

        #region SINGLETON
        if (instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        LoadScene();
        foreach (Modifier modifier in modifiers) {

        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start () {
        
    }

    public void SpawnBlueprint(Vector3 position) {
        GameObject newBlueprint = Instantiate(blueprint);
        newBlueprint.transform.position = position + new Vector3(0, 1, 0);
        int index = Random.Range(0, modifiers.Count);
        newBlueprint.GetComponent<BlueprintBehaviour>().modifier = modifiers[index];
        modifiers.RemoveAt(index);
    }

    public void LoadScene() {
        switch (scene) {
            case GameState.GAME:
                #region Setup Initial Room
                levelGenerator = GetComponent<LevelGenerator>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
                SpawnPlayer();

                player.GetComponent<Player>().SetupPlayer();
                #endregion

                #region Setup Camera
                Camera.main.GetComponent<CameraBehaviour>().player = player.transform;
                Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
                #endregion

                blueSouls = 0;
                greenSouls = 0;
                redSouls = 0;
            break;

            case GameState.LOBBY:
                #region Setup Initial Room
                currentRoom = GameObject.Find("Map");
                SpawnPlayer();
                #endregion
                foreach(GameObject crewMember in GameObject.FindGameObjectsWithTag("CrewMember")) {
                    if (blacksmith){
                        crewMember.SetActive(true);//if we have a list and not just a bool for each change this
                        crewMember.GetComponent<Animator>().SetBool("IDLE", true);
                    }
                    else {
                        crewMember.SetActive(false);//if we have a list and not just a bool for each change this
                    }
                }   
                player.GetComponent<Player>().SetupPlayer();

                #region Setup Camera
                Camera.main.GetComponent<CameraBehaviour>().player = player.transform;
                Camera.main.GetComponent<CameraBehaviour>().SetupCamera(currentRoom.transform.position);
                #endregion

            break;
        }
    }

    private void Update() {

        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("God Mode activated.");
            godMode = !godMode;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D)) {
            Debug.Log("Debug Mode activated.");
            debugMode = !debugMode;
        }

    }

    private GameObject SpawnLevel() {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer() {
        player = Instantiate(playerGO);
        player.transform.position = currentRoom.transform.position;
    }

    public void ChangeRoom(GameObject door) {

        Transform room = door.transform;
        while (room.parent != null) {
            room = room.parent.transform;
            if (room.tag == "Room") break;
        }

        currentRoom = room.gameObject;
        room.transform.Find("NavMesh").gameObject.SetActive(true);
        room.GetComponent<SpawnController>().Spawn(player);

    }

}
