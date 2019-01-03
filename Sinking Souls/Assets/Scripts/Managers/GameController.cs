using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    public enum GameState { LOBBY, GAME, ARENA, LOADSCENE, TABERN, MAIN_MENU };
    public GameState scene = GameState.LOBBY;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blueprint;

    [Header("Crew Members")]
    public bool blacksmith = false;
    public bool alchemist = false;

    [Header("Items")]
    [SerializeField] private int m_LobbySouls;
    public int LobbySouls
    {
        get { return m_LobbySouls; }
        set { m_LobbySouls = value; }
    }

    private int m_RunSouls;
    public int RunSouls
    {
        get { return m_RunSouls; }
        set { m_RunSouls = value; }
    }

    public List<Modifier> modifiers;
    public List<Ability> abilities;
    public List<Weapon> weapons;
    public List<Enhancer> enhancers;
    
    [HideInInspector] public bool died;
    [HideInInspector] public static GameController instance;
    [HideInInspector] public bool debugMode = false;
    [HideInInspector] public bool godMode = false;
    [HideInInspector] public GameObject currentRoom;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Text lobbySoulsHUD;

    private LevelGenerator levelGenerator;
    public LevelGenerator LevelGenerator
    {
        get { return levelGenerator;  }
    }

    private void Awake()
    {
        #region SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        #endregion

        levelGenerator = GetComponent<LevelGenerator>();
    }

    public void LoadScene()
    {
        switch (scene)
        {
            case GameState.MAIN_MENU:
                break;

            case GameState.TABERN:
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();

                GameObject.Find("Innkeeper").SetActive(true);

                SetupGame();
                player.GetComponent<Player>().Heal();

                break;

            case GameState.GAME:
                died = false;
                
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;

                SetupGame();

                break;

            case GameState.LOBBY:
                levelGenerator.currentLevel = -1;
                currentRoom = GameObject.Find("PlayerSpawn");

                SetupGame();

                if (died)
                {
                    m_RunSouls = 0;
                }
                else
                {
                    m_LobbySouls += m_RunSouls;
                    modifiers.FindAll(modifier => modifier.picked).ForEach(modifier => modifier.owned = true);
                }

                modifiers.ForEach(modifier => modifier.picked = false);

                lobbySoulsHUD = GameObject.Find("SoulsNumber").GetComponent<Text>();
                lobbySoulsHUD.text = LobbySouls.ToString();

                GameObject.Find("Blacksmith").SetActive(blacksmith);
                GameObject.Find("Alchemist").SetActive(alchemist);

                GameObject.Find("Alchemist").GetComponent<AlchemistBehaviour>().FillShop();


                levelGenerator.tabernaSpawned = false;
                break;

            case GameState.ARENA:
                currentRoom = GameObject.Find("Arena");

                SetupGame();

                godMode = true;
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugMode = !debugMode;

            string status = debugMode ? "activated" : "deactivated";
            Debug.Log("Debug mode " + status);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            godMode = !godMode;

            string status = godMode ? "activated" : "deactivated";
            Debug.Log("God mode " + status);
        }
    }

    #region SceneManagment Functions
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadScene();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ChangeScene(string sceneName)
    {
        switch (sceneName.ToUpper())
        {
            case "LOBBY":
                scene = GameState.LOBBY;
                break;

            case "LOADSCENE":
                scene = GameState.LOADSCENE;
                break;

            case "GAME":
                scene = GameState.GAME;
                break;

            case "TABERN":
                scene = GameState.TABERN;
                break;

            default:
                Debug.LogError("A scene named " + sceneName + " was not found.");
                return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ChangeScene(GameState newScene)
    {
        scene = newScene;
        switch (scene)
        {
            case GameState.LOBBY:
                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
                break;

            case GameState.GAME:
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
                break;

            case GameState.LOADSCENE:
                SceneManager.LoadScene("LoadScene", LoadSceneMode.Single);
                break;

            case GameState.TABERN:
                break;
            case GameState.MAIN_MENU:
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
                break;
            default:
                break;
        }

        
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
    #endregion

    #region Spawn Functions
    private GameObject SpawnLevel()
    {
        return levelGenerator.Spawn();
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab);
        player.transform.position = currentRoom.transform.position;
    }

    public void SpawnBlueprint(Vector3 position)
    {
        var possibleModifiers = modifiers.FindAll(modifer => !modifer.owned && !modifer.picked);
        if (possibleModifiers.Count == 0) return;

        var spawnedModifier = possibleModifiers[UnityEngine.Random.Range(0, possibleModifiers.Count - 1)];

        GameObject newBlueprint = Instantiate(blueprint);
        newBlueprint.transform.position = position + new Vector3(0, 1, 0);
        newBlueprint.GetComponent<BlueprintBehaviour>().modifier = spawnedModifier;
    }
    #endregion

    #region Setup Functions
    private void SetupGame()
    {
        SpawnPlayer();
        SetupCamera();
        SetupPlayer();
    }

    private void SetupCamera()
    {
        CameraManager.instance.player = player.transform;
        CameraManager.instance.SetupCamera(currentRoom.transform.position);
    }

    private void SetupPlayer()
    {
        player.GetComponent<Player>().SetupPlayer();
    }

    public void ChangeRoom(GameObject door)
    {
        Transform room = door.transform;
        while (room.parent != null)
        {
            room = room.parent.transform;
            if (room.tag == "Room") break;
        }

        currentRoom = room.gameObject;
        room.transform.Find("NavMesh").gameObject.SetActive(true);
        room.GetComponent<SpawnController>().Spawn(player);
    }
    #endregion

}
