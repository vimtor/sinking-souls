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

    public bool closing = false;
    private bool closed = false;
    private bool playSound;
    private bool opened;

    private void Start()
    {
        if (speed == 0) speed = 2.25f;
        openPosition = closedDoor[0].transform.position.y;
        closedPosition = closedDoor[0].transform.position.y + offset;
    }

    public bool checkEnemyDistance(GameObject en) {

        if (GameController.instance.m_RescuedAlchemist)
            return Vector3.Distance(transform.position, en.transform.position) < 21;     //Death island
        else return Vector3.Distance(transform.position, en.transform.position) < 12;    //Tritton island

    }

    public bool checkPlayerdistAndEnemies() {
        if (checkDistance()) {
            foreach(GameObject en in GameController.instance.roomEnemies) {
                if (checkEnemyDistance(en)) return true;
            }
            return false;
        }
        return false;
    }

    public bool noActiveAI() {
        foreach (GameObject en in GameController.instance.roomEnemies) {
            if (en.GetComponent<AIController>().aiActive) return false;
        }
        return true;
    } 
    
    void Update ()
    {
        if (checkPlayerdistAndEnemies()) {
            if (!closed) closeDoor();
        }

        if(noActiveAI() && !opened) openDoor();

        if (closed)
        {
            playSound = false;
            closing = false;
        }

        if (opened)
        {
            playSound = false;
        }
    }


    void closeDoor()
    {
        if (!playSound)
        {
            playSound = true;

            AudioManager.Instance.PlayEffect("Wall");
        }
        

        if (checkDistance()) {
            for (int i = 0; i < closedDoor.Count; i++)// GameObject door in closedDoor
            {
                if (closedDoor[i].transform.position.y < closedPosition) {
                    closedDoor[i].transform.position += Vector3.up * Time.deltaTime * speed;
                    closing = true;

                }
                else {
                    closedDoor[i].transform.position = new Vector3(closedDoor[i].transform.position.x, closedPosition, closedDoor[i].transform.position.z);
                    closed = true;
                    //closing = false;
                }
            }
        }

        opened = false;
    }

   public  bool checkDistance()
    {
        if(GameController.instance.m_RescuedAlchemist)
        return Vector3.Distance(transform.position, GameController.instance.player.transform.position) < 20;
        else return Vector3.Distance(transform.position, GameController.instance.player.transform.position) < 11;
    }

    void openDoor()
    {
        if (!playSound)
        {
            playSound = true;

            AudioManager.Instance.PlayEffect("Wall");
        }

        foreach (GameObject door in closedDoor)
        {
            if (door.transform.position.y  > openPosition)
            {
                door.transform.position -= Vector3.up * Time.deltaTime * speed;
                Debug.Log("Abriendo");
            }
            else
            {
                door.transform.position = new Vector3(door.transform.position.x, openPosition, door.transform.position.z);
                opened = true;
            }
        }

        closed = false;
    }
}
