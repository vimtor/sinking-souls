using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    Room[,] grid;
    List<Vector2> takenPos = new List<Vector2>();

    public int gridSizeX = 10, gridSizeY = 10;
    public int numberRooms = 5;

    public List<GameObject> RoomsA = new List<GameObject>();
    public List<GameObject> RoomsB = new List<GameObject>();
    public List<GameObject> RoomsC = new List<GameObject>();
    public List<GameObject> RoomsD = new List<GameObject>();
    public List<GameObject> RoomsE = new List<GameObject>();
	
	void Start () {
        if (CheckRooms()) {
            CreateRooms();
            SetRoomDoors();
            CreateMap();
        }
        else {
            Debug.LogError("Number of rooms specified don't fit into the grid.");
        }
	}

    private bool CheckRooms() {
        return (gridSizeX * gridSizeY) > numberRooms;
    }

    private void CreateRooms() {

        // Create an empty array of rooms.
        grid = new Room[gridSizeX, gridSizeY];

        // The center room is initialized to Vector2.zero because in the actual game, 
        // you want it to be centered in the coordinate system. 
        grid[gridSizeX / 2, gridSizeY / 2] = new Room(Vector2.zero, Room.RoomType.COMBAT);
        takenPos.Insert(0, Vector2.zero);

        Vector2 newPos = Vector2.zero;
        for (int i = 0; i < numberRooms - 1; i++) {
            
            // Get a new random position for a room.
            newPos = NewPosition();

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

    private void CreateMap() {
        int roomSize = 10/2;
        int roomCount = 1;
        Debug.Log("Map created successfully.");

        foreach(Room room in grid) {
            if (room == null) continue;
            GameObject currentRoom;
            //all on true
            if(room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == true) { 
                room.prefab = RoomsE[Random.Range(0,RoomsE.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            //3 on true 1 on false
            else if(room.doorTop == true && room.doorBot == true && room.doorLeft == true && room.doorRight == false) {
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

            //2 on true 2 on false
                // confronted
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
                //corner
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

            //3 on false 1 true
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == true && room.doorRight == false) {
                room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                //room.prefab.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
            }
            else if (room.doorTop == true && room.doorBot == false && room.doorLeft == false && room.doorRight == false) {
                room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                //room.prefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }
            else if (room.doorTop == false && room.doorBot == false && room.doorLeft == false && room.doorRight == true) {
                room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                //room.prefab.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
            }
            else if (room.doorTop == false && room.doorBot == true && room.doorLeft == false && room.doorRight == false) {
                room.prefab = RoomsA[Random.Range(0, RoomsA.Count - 1)];
                currentRoom = SpawnRoom(room, roomSize, roomCount);
                currentRoom.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //room.prefab.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }

            //SpawnRoom(room, roomSize, roomCount);
            roomCount++;
        }
    }

    private GameObject SpawnRoom(Room room, int roomSize, int roomCount) {
        Vector3 realPosition = new Vector3(room.gridPos.x, 0, room.gridPos.y);
        realPosition *= roomSize;
        room.prefab.transform.position = transform.position + realPosition;

        GameObject _room = Instantiate(room.prefab, room.prefab.transform);
        _room.name = "Room_" + roomCount;
        _room.transform.parent = gameObject.transform;
        Debug.Log(_room.name + " " + room.gridPos + " " + realPosition);
        return _room;

    }
}
