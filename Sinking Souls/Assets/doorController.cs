using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorController : MonoBehaviour
{
    public GameObject roomIcon;
    public GameObject roomUnknown;

    public List<GameObject> closedDoor;
    private float closedPosition;
    private float openPosition;
    public float speed = 2.25f;
    public float offset = 4.5f;

    private bool closed = false;

    private void Start()
    {
        if (speed == 0) speed = 2.25f;
        openPosition = closedDoor[0].transform.position.y;
        closedPosition = closedDoor[0].transform.position.y + offset;
    }

    // Update is called once per frame
    void Update () {

        if (GameController.instance.roomEnemies.Count > 0) {
            if (!closed) closeDoor();
        }
        else openDoor();

	}


    void closeDoor()
    {

        // AudioManager.Instance.PlayEffect("Wall");

        if(checkDistance())
            for (int i = 0; i < closedDoor.Count; i++)// GameObject door in closedDoor
            {
                if (closedDoor[i].transform.position.y < closedPosition) {
                    closedDoor[i].transform.position += Vector3.up * Time.deltaTime * speed;
                    Debug.Log("Cerrando");
                }
                else {
                    closedDoor[i].transform.position = new Vector3(closedDoor[i].transform.position.x, closedPosition, closedDoor[i].transform.position.z);
                    closed = true;
                }
            }
        
    }

    bool checkDistance()
    {
        if(GameController.instance.m_RescuedAlchemist)
        return Vector3.Distance(transform.position, GameController.instance.player.transform.position) < 20; //Por que 38? porque lo digo yo
        else return Vector3.Distance(transform.position, GameController.instance.player.transform.position) < 11;
    }

    void openDoor()
    {
        // AudioManager.Instance.PlayEffect("Wall");

        foreach (GameObject door in closedDoor)
        {
            if (door.transform.position.y  > openPosition)
            {
                door.transform.position -= Vector3.up * Time.deltaTime * speed;
                Debug.Log("Abriendo");
            }
            else door.transform.position = new Vector3(door.transform.position.x, openPosition, door.transform.position.z);
        }
        closed = false;
    }
}
