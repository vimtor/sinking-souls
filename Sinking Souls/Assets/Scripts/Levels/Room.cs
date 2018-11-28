using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {

    public enum RoomType {
        COMBAT,
        BOSS,
        INITIAL,
        ELITE
    }

    public Vector2 gridPos;
    public RoomType type;
    public bool doorTop, doorBot, doorLeft, doorRight;
    public GameObject prefab;

	public Room(Vector2 _gridPos, RoomType _type) {
        gridPos = _gridPos;
        type = _type;
    }

}
