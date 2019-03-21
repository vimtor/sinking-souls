using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    #region Minimap Configuration
    [Header("Minimap Configuration")]
    public GameObject roomIconPrefab;
    public float roomIconHeight;

    public Material combatRoomIcon;
    public Material bossRoomIcon;
    public Material eliteRoomIcon;
    public Material initialRoomIcon;
    public Material nextFloorRoomIcon;
    #endregion

    #region Level Generator Configuration
    [Header("Level Generator Configuration")]
    public bool tabernaSpawned = false;
    public int currentLevel = -1;
    public int numberRooms = 5;
    public GameObject lastRoom;

    [Tooltip("Rate at which elites spawn.")]
    public float eliteRate;
    public float roomSize;

    [Tooltip("By default the seed will generate randomly if not specified. " +
             "The inputted seed can be any combination of numbers and character.")]
    public string seed;
    #endregion

    #region Rooms Setup
    [Header("Rooms Setup")]

    public LevelGeneratiorConfiguration level;

    private List<ListWrapper> RoomsA = new List<ListWrapper>();
    private List<ListWrapper> RoomsB = new List<ListWrapper>();
    private List<ListWrapper> RoomsC = new List<ListWrapper>();
    private List<ListWrapper> RoomsD = new List<ListWrapper>();
    private List<ListWrapper> RoomsE = new List<ListWrapper>();

    private List<ListWrapper> RoomsEliteA = new List<ListWrapper>();
    private List<ListWrapper> RoomsEliteB = new List<ListWrapper>();
    private List<ListWrapper> RoomsEliteC = new List<ListWrapper>();
    private List<ListWrapper> RoomsEliteD = new List<ListWrapper>();
    private List<ListWrapper> RoomsEliteE = new List<ListWrapper>();

    private List<GameObject> RoomsBoss = new List<GameObject>();
    private List<GameObject> NextLevelRoom = new List<GameObject>();
    private List<GameObject> Taberns = new List<GameObject>();

    public List<SpawnerConfiguration> Crew = new List<SpawnerConfiguration>();

    #endregion

    [HideInInspector] public List<Vector2> takenPos = new List<Vector2>();

    private Room[,] grid;
    private int gridSizeX, gridSizeY;
    private GameObject levelWrapper;
    private bool elitePlaced = false;

    void Start () {
        if(numberRooms <= 0) {
            Debug.LogError("The number of rooms specified is not valid.");
        }
    }

    public GameObject Spawn() {

        RoomsA = level.RoomsA;
        RoomsB = level.RoomsB;
        RoomsC = level.RoomsC;
        RoomsD = level.RoomsD;
        RoomsE = level.RoomsE;

        RoomsEliteA = level.RoomsEliteA;
        RoomsEliteB = level.RoomsEliteB;
        RoomsEliteC = level.RoomsEliteC;
        RoomsEliteD = level.RoomsEliteD;
        RoomsEliteE = level.RoomsEliteE;

        RoomsBoss = level.RoomsBoss;
        NextLevelRoom = level.NextLevelRoom;
        Taberns = level.Taberns;

        levelWrapper = Instantiate(new GameObject());
        levelWrapper.name = "Level Wrapper";
        if (currentLevel == 2 && !tabernaSpawned)
        {
            Debug.Log("Should have spawned a taberna");
            Room room = new Room( new Vector2(0,0), Room.RoomType.INITIAL);
            room.prefab = Taberns[0];
            tabernaSpawned = true;
            return SpawnRoom(room, roomSize, -1);

        }
        else { 

            SetSeed();


            SetGridSize();
            CreateRooms();
            if (Random.value < eliteRate) SetEliteRoom();
            SetRoomDoors();
            if (currentLevel == 3) PlaceBossRoom();
            else PlaceNextLevelRoom();
            return CreateMap();
        }
    }


    private void PlaceNextLevelRoom()
    {
        Vector2 bossRoomPosition = Vector2.zero;
        Vector2 initialRoomPosition = new Vector2(gridSizeX / 2, gridSizeY / 2);
        float maxDistance = 0;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeX; y++)
            {
                if (grid[x, y] != null)
                {
                    Vector2 newPos = new Vector2(x, y);
                    if (NumberOfNeighbors(newPos) == 1 && grid[(int)newPos.x, (int)newPos.y].type != Room.RoomType.INITIAL)
                    {
                        float distance = Vector2.Distance(newPos, initialRoomPosition);
                        if (distance > maxDistance)
                        {
                            maxDistance = distance;
                            bossRoomPosition = newPos;
                        }
                    }
                }
            }
        }
        grid[(int)bossRoomPosition.x, (int)bossRoomPosition.y].type = Room.RoomType.NEXT_FLOOR;
    }

    private void SetSeed() {
        seed = seed.Replace(" ", "");
        if (seed == null || seed != "") {
            Random.InitState(seed.GetHashCode());
        }
    }

    private void SetGridSize() {
        gridSizeX = gridSizeY = numberRooms * 2 + 1;
    }

    private void SetEliteRoom() {
        int x;
        Vector2 aux;
        do
        {
            x = Random.Range(0, takenPos.Count);
            aux = takenPos[x];
        } while (grid[(int)aux.x, (int)aux.y].type == Room.RoomType.INITIAL);
            grid[(int)aux.x, (int)aux.y].type = Room.RoomType.ELITE;      
    }

    private void CreateRooms() {
        // Create an empty array of rooms.
        grid = new Room[gridSizeX, gridSizeY];

        // The center room is initialized to Vector2.zero because in the actual game, 
        // you want it to be centered in the coordinate system. 
        grid[gridSizeX / 2, gridSizeY / 2] = new Room(new Vector2(gridSizeX / 2, gridSizeY / 2), Room.RoomType.INITIAL);
        takenPos.Insert(0, new Vector2 (gridSizeX / 2, gridSizeY / 2));

        Vector2 newPos = Vector2.zero;
        for (int i = 0; i < numberRooms - 1; i++) {
            int countsOfDowhile = 0;
            do {
                countsOfDowhile++;
                // Get a new random position for a room.
                newPos = NewPosition();
            } while (NumberOfNeighbors(newPos) > 1);
            

            // Add it to the grid and to the taken positions list.
            grid[(int) newPos.x, (int) newPos.y] = new Room(newPos, Room.RoomType.COMBAT);
            takenPos.Insert(0, newPos);

        }
    }

    private Vector2 NewPosition() {
        int x, y;
        int index;

        Vector2 newPos = Vector2.zero;

        do {
            // Get a random index of one of the rooms taken.
            index = Random.Range(0, takenPos.Count - 1);

            // Get the coordinates of the randomly selected room.
            x = (int)takenPos[index].x;
            y = (int)takenPos[index].y;

            // Decide a random room adjacent to the selected one.
            if (Random.value < 0.5f) {
                x += Random.value < 0.5f ? 1 : -1;
            }
            else {
                y += Random.value < 0.5f ? 1 : -1;
            }

            // Add the new pos to a vector to check if the takenPos list contains the new position.
            newPos = new Vector2(x, y);

        } while (takenPos.Contains(newPos) || x >= gridSizeX || x < 0 || y >= gridSizeY || y < 0);
        // Repeat the loop until a position that isn't cointained in the list and isn't surpassing the grid limits is found.

        return newPos;
    }

    private void SetRoomDoors() {
        // Iterate over the whole grid to set the doors.
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeX; y++) {
                // Go to the next iteration if null.
                if (grid[x, y] == null) continue;

                // Checking below.
                if (y - 1 < 0) {
                    grid[x, y].doorBot = false;
                }
                else {
                    grid[x, y].doorBot = (grid[x, y - 1] != null);
                }

                // Checking above.
                if (y + 1 >= gridSizeY) {
                    grid[x, y].doorTop = false;
                }
                else {
                    grid[x, y].doorTop = (grid[x, y + 1] != null);
                }

                // Checking left.
                if (x - 1 < 0) {
                    grid[x, y].doorLeft = false;
                }
                else {
                    grid[x, y].doorLeft = (grid[x - 1, y] != null);
                }

                // Checking right.
                if (x + 1 >= gridSizeX) {
                    grid[x, y].doorRight = false;
                }
                else {
                    grid[x, y].doorRight = (grid[x + 1, y] != null);
                }
            }
        }
    }

    private void PlaceBossRoom() {

        Vector2 bossRoomPosition = Vector2.zero;
        Vector2 initialRoomPosition = new Vector2(gridSizeX / 2, gridSizeY / 2);
        float maxDistance = 0;
        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeX; y++) {
                if (grid[x, y] != null) { 
                    Vector2 newPos = new Vector2(x, y);
                    if (NumberOfNeighbors(newPos) == 1 && grid[(int)newPos.x, (int)newPos.y].type != Room.RoomType.INITIAL) {
                        float distance = Vector2.Distance(newPos, initialRoomPosition);
                        if (distance > maxDistance) {
                            maxDistance = distance;
                            bossRoomPosition = newPos;
                        }
                    }
                }
            }
        }
        grid[(int)bossRoomPosition.x, (int)bossRoomPosition.y].type = Room.RoomType.BOSS;
    }

    private GameObject CreateMap() {

        int roomCount = 1;
        GameObject initialRoom = null;

        foreach(Room room in grid) {
            if (room == null) continue;
            GameObject currentRoom = null;

            #region ROOMS E
            if(room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == true) { 
                if(room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteE[currentLevel].List[Random.Range(0, RoomsEliteE[currentLevel].List.Count - 1)];
                else room.prefab = RoomsE[currentLevel].List[Random.Range(0, RoomsE[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            #endregion

            #region ROOMS D
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteD[currentLevel].List[Random.Range(0, RoomsEliteD[currentLevel].List.Count - 1)];
                else room.prefab = RoomsD[currentLevel].List[Random.Range(0, RoomsD[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180 + 180, 0));
            }
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == false && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteD[currentLevel].List[Random.Range(0, RoomsEliteD[currentLevel].List.Count - 1)];
                else room.prefab = RoomsD[currentLevel].List[Random.Range(0, RoomsD[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0 + 180, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == true && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteD[currentLevel].List[Random.Range(0, RoomsEliteD[currentLevel].List.Count - 1)];
                else room.prefab = RoomsD[currentLevel].List[Random.Range(0, RoomsD[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270 + 180, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == true && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteD[currentLevel].List[Random.Range(0, RoomsEliteD[currentLevel].List.Count - 1)];
                else room.prefab = RoomsD[currentLevel].List[Random.Range(0, RoomsD[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90 + 180, 0));
            }
            #endregion

            #region ROOMS C
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == false && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteC[currentLevel].List[Random.Range(0, RoomsEliteC[currentLevel].List.Count - 1)];
                else room.prefab = RoomsC[currentLevel].List[Random.Range(0, RoomsC[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == true && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteC[currentLevel].List[Random.Range(0, RoomsEliteC[currentLevel].List.Count - 1)];
                else room.prefab = RoomsC[currentLevel].List[Random.Range(0, RoomsC[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            #endregion

            #region ROOMS B
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == false && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteB[currentLevel].List[Random.Range(0, RoomsEliteB[currentLevel].List.Count - 1)];
                else room.prefab = RoomsB[currentLevel].List[Random.Range(0, RoomsB[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90 + 90, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == false && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteB[currentLevel].List[Random.Range(0, RoomsEliteB[currentLevel].List.Count - 1)];
                else room.prefab = RoomsB[currentLevel].List[Random.Range(0, RoomsB[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180 + 90, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == true && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteB[currentLevel].List[Random.Range(0, RoomsEliteB[currentLevel].List.Count - 1)];
                else room.prefab = RoomsB[currentLevel].List[Random.Range(0, RoomsB[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270 + 90, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == true && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteB[currentLevel].List[Random.Range(0, RoomsEliteB[currentLevel].List.Count - 1)];
                else room.prefab = RoomsB[currentLevel].List[Random.Range(0, RoomsB[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0 + 90, 0));
            }
            #endregion

            #region ROOMS A
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == true && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteA[currentLevel].List[Random.Range(0, RoomsEliteA[currentLevel].List.Count - 1)];
                else if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsBoss.Count - 1)];
                else if (room.type == Room.RoomType.NEXT_FLOOR) room.prefab = NextLevelRoom[Random.Range(0, NextLevelRoom.Count - 1)];
                else room.prefab = RoomsA[currentLevel].List[Random.Range(0, RoomsA[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == false && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteA[currentLevel].List[Random.Range(0, RoomsEliteA[currentLevel].List.Count - 1)];
                else if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsBoss.Count - 1)];
                else if (room.type == Room.RoomType.NEXT_FLOOR) room.prefab = NextLevelRoom[Random.Range(0, NextLevelRoom.Count - 1)];
                else room.prefab = RoomsA[currentLevel].List[Random.Range(0, RoomsA[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == false && room.doorRight == true) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteA[currentLevel].List[Random.Range(0, RoomsEliteA[currentLevel].List.Count - 1)];
                else if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsBoss.Count - 1)];
                else if (room.type == Room.RoomType.NEXT_FLOOR) room.prefab = NextLevelRoom[Random.Range(0, NextLevelRoom.Count - 1)];
                else room.prefab = RoomsA[currentLevel].List[Random.Range(0, RoomsA[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == false && room.doorRight == false) {
                if (room.type == Room.RoomType.ELITE) room.prefab = RoomsEliteA[currentLevel].List[Random.Range(0, RoomsEliteA[currentLevel].List.Count - 1)];
                else if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsBoss.Count - 1)];
                else if (room.type == Room.RoomType.NEXT_FLOOR) room.prefab = NextLevelRoom[Random.Range(0, NextLevelRoom.Count - 1)];
                else room.prefab = RoomsA[currentLevel].List[Random.Range(0, RoomsA[currentLevel].List.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

            if (room.type == Room.RoomType.INITIAL) {
                initialRoom = currentRoom;
            }

            #endregion

            roomCount++;

        }

        if(GameController.instance.debugMode) Debug.Log("Map created successfully.");
        return initialRoom;
    }

    private GameObject SpawnRoom(Room room, float roomSize, int roomCount) {
        Vector3 realPosition = new Vector3(room.gridPos.x, 0, room.gridPos.y);
        realPosition *= roomSize;
        GameObject instantiatedRoom = Instantiate(room.prefab, room.prefab.transform, true);
        instantiatedRoom.transform.position = realPosition - (new Vector3(15,0,15) * roomSize);
        instantiatedRoom.name = "Room_" + roomCount;
        instantiatedRoom.transform.parent = levelWrapper.transform;

        #region Room Minimap
        GameObject roomIcon = Instantiate(roomIconPrefab, instantiatedRoom.transform);
        roomIcon.name = "RoomIcon";
        Vector3 newPos = roomIcon.transform.position;
        newPos.y += roomIconHeight;
        roomIcon.transform.position = newPos;
        roomIcon.SetActive(false);
        #endregion

        switch (room.type){
            case Room.RoomType.COMBAT:
                roomIcon.GetComponent<MeshRenderer>().material = combatRoomIcon;
                break;

            case Room.RoomType.BOSS:
                for(int i = 0; i < instantiatedRoom.transform.childCount; i++)
                {
                    if (instantiatedRoom.transform.GetChild(i).name == "Door") lastRoom = instantiatedRoom.transform.GetChild(i).gameObject;
                }

                roomIcon.GetComponent<MeshRenderer>().material = bossRoomIcon;

                //instantiatedRoom.GetComponent<SpawnController>().possibleConfigurations.Clear();
                //instantiatedRoom.GetComponent<SpawnController>().possibleConfigurations.Add(Crew[0]); //index depending on wich level you are
                break;

            case Room.RoomType.ELITE:
                roomIcon.GetComponent<MeshRenderer>().material = eliteRoomIcon;
                break;

            case Room.RoomType.INITIAL:
                roomIcon.GetComponent<MeshRenderer>().material = initialRoomIcon;
                break;

            case Room.RoomType.NEXT_FLOOR:
                for (int i = 0; i < instantiatedRoom.transform.childCount; i++)
                {
                    if (instantiatedRoom.transform.GetChild(i).name == "Door")
                    {
                        lastRoom = instantiatedRoom.transform.GetChild(i).gameObject;
                    }
                }
                roomIcon.GetComponent<MeshRenderer>().material = nextFloorRoomIcon;
                break;

            default:
                break;
        }


        return instantiatedRoom;
    }



    private int NumberOfNeighbors(Vector2 checkingPos) {
        int neighbors = 0;

        if (takenPos.Contains(checkingPos + Vector2.up)) neighbors++;
        if (takenPos.Contains(checkingPos + Vector2.down)) neighbors++;
        if (takenPos.Contains(checkingPos + Vector2.left)) neighbors++;
        if (takenPos.Contains(checkingPos + Vector2.right)) neighbors++;

        return neighbors;
    }
}
