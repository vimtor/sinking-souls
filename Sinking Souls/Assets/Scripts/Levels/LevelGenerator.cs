using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public int numberRooms = 5;

    [Tooltip("By default the seed will generate randomly if not specified. " +
             "The inputted seed can be any combination of numbers and character.")]
    public string seed;

    public List<GameObject> RoomsA = new List<GameObject>();
    public List<GameObject> RoomsB = new List<GameObject>();
    public List<GameObject> RoomsC = new List<GameObject>();
    public List<GameObject> RoomsD = new List<GameObject>();
    public List<GameObject> RoomsE = new List<GameObject>();
    public List<GameObject> RoomsBoss = new List<GameObject>();

    private Room[,] grid;
    private List<Vector2> takenPos = new List<Vector2>();
    private int gridSizeX, gridSizeY;
    private GameObject levelWrapper;

    void Start () {
        if(numberRooms <= 0) {
            Debug.LogError("The number of rooms specified is not valid.");
        }
    }

    public GameObject Spawn() {
        SetSeed();

        levelWrapper = Instantiate(new GameObject());
        levelWrapper.name = "Level Wrapper";

        SetGridSize();
        CreateRooms();
        SetRoomDoors();
        PlaceBossRoom();
        return CreateMap();
    }

    private void SetSeed() {
        seed = seed.Replace(" ", "");
        if (seed != null || seed == "") Random.InitState(seed.GetHashCode());
    }

    private void SetGridSize() {
        gridSizeX = gridSizeY = numberRooms * 2 + 1;
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

            do {
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
        int roomSize = 30/2;
        int roomCount = 1;
        GameObject initialRoom = null;

        foreach(Room room in grid) {
            if (room == null) continue;
            GameObject currentRoom = null;

            #region ROOMS E
            if(room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == true) { 
                room.prefab = RoomsE[Random.Range(0,RoomsE.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            #endregion

            #region ROOMS D
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == false) {
                room.prefab = RoomsD[Random.Range(0, RoomsD.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == false && room.doorRight == true) {
                room.prefab = RoomsD[Random.Range(0, RoomsD.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == true && room.doorRight == true) {
                room.prefab = RoomsD[Random.Range(0, RoomsD.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == true && room.doorRight == true) {
                room.prefab = RoomsD[Random.Range(0, RoomsD.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            #endregion

            #region ROOMS C
            else if (room.doorTop == true && room.doorBot == true && room.doorLeft == false && room.doorRight == false) {
                room.prefab = RoomsC[Random.Range(0, RoomsC.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == true && room.doorRight == true) {
                room.prefab = RoomsC[Random.Range(0, RoomsC.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            #endregion

            #region ROOMS B
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == false && room.doorRight == true) {
                room.prefab = RoomsB[Random.Range(0, RoomsB.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == false && room.doorRight == true) {
                room.prefab = RoomsB[Random.Range(0, RoomsB.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == true && room.doorRight == false) {
                room.prefab = RoomsB[Random.Range(0, RoomsB.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == true && room.doorRight == false) {
                room.prefab = RoomsB[Random.Range(0, RoomsB.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            #endregion

            #region ROOMS A
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == true && room.doorRight == false) {
                if(room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsA.Count - 1)];
                else room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == false && room.doorRight == false) {
                if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsA.Count - 1)];
                else room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == false && room.doorRight == true) {
                if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsA.Count - 1)];
                else room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == false && room.doorRight == false) {
                if (room.type == Room.RoomType.BOSS) room.prefab = RoomsBoss[Random.Range(0, RoomsA.Count - 1)];
                else room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
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

    private GameObject SpawnRoom(Room room, int roomSize, int roomCount) {
        Vector3 realPosition = new Vector3(room.gridPos.x, 0, room.gridPos.y);
        realPosition *= roomSize;
        room.prefab.transform.position = transform.position + realPosition;

        GameObject instantiatedRoom = Instantiate(room.prefab, room.prefab.transform);
        instantiatedRoom.name = "Room_" + roomCount;
        instantiatedRoom.transform.parent = levelWrapper.transform;
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
