using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public enum GameState { LOBBY, GAME, ARENA, LOADSCENE, TABERN, MAIN_MENU };
    public GameState scene = GameState.LOBBY;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blueprint;

    #region Crew Members
    [Header("Crew Members")]
    public bool m_RescuedBlacksmith = false;
    public bool m_RescuedAlchemist = false;

    private GameObject m_BlacksmithObject;
    private GameObject m_AlchemistObject;
    #endregion

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

    public Modifier[] modifiers;
    public Ability[] abilities;
    public Weapon[] weapons;
    public Enhancer[] enhancers;
    public GameObject innKeeperShop;
    public List<GameObject> roomEnemies;
    
    [HideInInspector] public bool died;
    [HideInInspector] public bool inTavern;
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

        Cursor.visible = false;
        levelGenerator = GetComponent<LevelGenerator>();
    }

    public void LoadScene()
    {
        switch (scene)
        {
            case GameState.MAIN_MENU:
                break;

            case GameState.TABERN:
                inTavern = true;
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                GameObject InnKeeper = GameObject.Find("Triton Innkeeper");
                GameObject pannel = Instantiate(innKeeperShop, GameObject.Find("Canvas").transform, false);
                InnKeeper.GetComponent<InnkeeperBehaviour>().m_ShopPanel = pannel;
                InnKeeper.GetComponent<InnkeeperBehaviour>().m_EventSystem = EventSystem.current;

                SetupGame();
                player.GetComponent<Player>().Heal();

                GameObject.Find("Triton Innkeeper").GetComponent<InnkeeperBehaviour>().FillShop();
                break;

            case GameState.GAME:
                died = false;
                
                levelGenerator.takenPos = new List<Vector2>();
                currentRoom = SpawnLevel();
                currentRoom.GetComponent<SpawnController>().alreadySpawned = true;

                SetupGame();
                break;

            case GameState.LOBBY:
                inTavern = false;
                m_RunSouls = 0;

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

                    var pickedModifiers = Array.FindAll(modifiers, modifier => modifier.picked);
                    Array.ForEach(pickedModifiers, modifier => modifier.owned = true);
                }

                Array.ForEach(modifiers, modifier => modifier.picked = false);

                #region Crew Members
                m_BlacksmithObject = GameObject.Find("Galen");
                m_AlchemistObject = GameObject.Find("Ailin");

                m_BlacksmithObject.SetActive(m_RescuedBlacksmith);
                m_AlchemistObject.SetActive(m_RescuedAlchemist);

                if (m_RescuedBlacksmith) m_BlacksmithObject.GetComponent<BlacksmithBehaviour>().FillShop();
                if (m_RescuedAlchemist) m_AlchemistObject.GetComponent<AlchemistBehaviour>().FillShop();
                #endregion

                levelGenerator.tabernaSpawned = false;
                SaveManager.Save();
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

        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.transform.position = GetComponent<LevelGenerator>().lastRoom.GetComponent<DoorBehaviour>().nextDoor.transform.position;
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
        var possibleModifiers = Array.FindAll(modifiers, modifer => !modifer.owned && !modifer.picked);
        if (possibleModifiers.Length == 0) return;

        var spawnedModifier = possibleModifiers[UnityEngine.Random.Range(0, possibleModifiers.Length - 1)];

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

    public bool CanBuy(int price)
    {
        return (m_LobbySouls - price) > 0;
    }

    public void LoadGame()
    {
        var save = SaveManager.Load();
        if (save == null) return;

        m_LobbySouls = save.souls;
        m_RunSouls = save.runSouls;
        m_RescuedAlchemist = save.alchemist;
        m_RescuedBlacksmith = save.blacksmith;
    }

}
