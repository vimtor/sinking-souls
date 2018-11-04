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
    [HideInInspector] public List<Modifier> runModifiers;
    [HideInInspector] public List<Modifier> pickedModifiers;

    public bool blacksmith = false; // Consider making this a array that holds the unlocked/locked state of each friend.
    public GameObject blueprint;
    public List<Modifier> modifiers;
    public int blueSouls;
    public int redSouls;
    public int greenSouls;
    public int fullSouls;
    public List<SoulsUI> soulsUI;
    public bool died;

    private LevelGenerator levelGenerator;
    private GameObject shop;

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
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void SpawnBlueprint(Vector3 position) {
        if (runModifiers.Count != 0) {
            GameObject newBlueprint = Instantiate(blueprint);
            newBlueprint.transform.position = position + new Vector3(0, 1, 0);
            int index = Random.Range(0, runModifiers.Count);
            newBlueprint.GetComponent<BlueprintBehaviour>().modifier = runModifiers[index];
            runModifiers.RemoveAt(index);
        }
        else Debug.Log("No more blueprints to spawn");
    }

    public void UpdateUI() {
        foreach(SoulsUI UI in soulsUI) {
            UI.UpdateText();
        }
    }

    public void LoadScene() {
        switch (scene) {
            case GameState.GAME:
                died = false;
                #region Setup Initial Room
                levelGenerator = GetComponent<LevelGenerator>();
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;
                SpawnPlayer();

                player.GetComponent<Player>().SetupPlayer();
                #endregion

                #region Setup Camera
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
            #endregion

                soulsUI = new List<SoulsUI>();

                for (int i = 0; i < 2; i++) {//change this depending on how meny blueprints we want to spawn on a game
                    do {
                        int index = Random.Range(0, modifiers.Count);
                    } while (runModifiers.Contains(modifiers[i]));///|| modifiers[i].unlocked));
                    runModifiers.Add(modifiers[i]);
                }   
                blueSouls = 0;
                greenSouls = 0;
                redSouls = 0;
                foreach(GameObject Go in GameObject.FindGameObjectsWithTag("SoulUI")) soulsUI.Add(Go.GetComponent<SoulsUI>());
            break;

            case GameState.LOBBY:
                #region Setup Initial Room
                currentRoom = GameObject.Find("Map");
                SpawnPlayer();
            #endregion

                if (!died) {
                    fullSouls = blueSouls < greenSouls ? blueSouls : greenSouls;
                    fullSouls = fullSouls < redSouls ? fullSouls : redSouls;
                }
                else {
                    blueSouls = 0;
                    greenSouls = 0;
                    redSouls = 0;
                    foreach (Modifier mod in pickedModifiers) mod.unlocked = false;
                }
                runModifiers = new List<Modifier>();
                pickedModifiers = new List<Modifier>();

                foreach (GameObject crewMember in GameObject.FindGameObjectsWithTag("CrewMember")) {
                    if (blacksmith){
                        crewMember.SetActive(true);//if we have a list and not just a bool for each change this
                        crewMember.GetComponent<Animator>().SetBool("IDLE", true);
                    }
                    else {
                        crewMember.SetActive(false);//if we have a list and not just a bool for each change this
                    }
                }   
                player.GetComponent<Player>().SetupPlayer();

                GameObject.FindGameObjectWithTag("Shop").GetComponent<Shop>().FillShop();
                
                #region Setup Camera
                CameraManager.instance.player = player.transform;
                CameraManager.instance.SetupCamera(currentRoom.transform.position);
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
