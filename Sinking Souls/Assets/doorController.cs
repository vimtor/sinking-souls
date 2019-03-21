using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour {

    public List<GameObject> closedDoor;
    private float closedPosition;
    private float openPosition;
    private float speed;


    private void Start()
    {
        openPosition = -103;
        closedPosition = 92;
        speed = 90;
    }

    // Update is called once per frame
    void Update () {
        if (GameController.instance.roomEnemies.Count > 0) closeDoor();
        else openDoor();
	}


    void closeDoor()
    {
        for (int i = 0; i < closedDoor.Count; i++)// GameObject door in closedDoor)
        {
            if(closedDoor[i].transform.localPosition.y < closedPosition)
            {
                closedDoor[i].transform.localPosition += Vector3.up * Time.deltaTime * speed;
            }
            else closedDoor[i].transform.localPosition = new Vector3(closedDoor[i].transform.localPosition.x, closedPosition, closedDoor[i].transform.localPosition.z);
        }
    }

    void openDoor()
    {
        foreach (GameObject door in closedDoor)
        {
            if (door.transform.localPosition.y  > openPosition)
            {
                door.transform.localPosition -= Vector3.up * Time.deltaTime * speed;
            }
            else door.transform.localPosition = new Vector3(door.transform.localPosition.x, openPosition, door.transform.localPosition.z);
        }
    }
}
