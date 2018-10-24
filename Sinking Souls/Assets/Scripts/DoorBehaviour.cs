using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    public GameObject nextDoor;
    public bool locked = false;//change this by the gameController

    private GameController GC;

	void Start () {
        SetNext();

        GameObject aux = gameObject;
        while (aux.transform.parent != null) {
            aux = aux.transform.parent.gameObject;
            if (aux.tag == "GameController") break;
        }
        GC = aux.GetComponent<GameController>();

    }

    void SetNext() {
        int layerMask = 1 << 13;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            nextDoor = hit.transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !locked) {
            Debug.Log("TP");
            other.GetComponent<Transform>().position = nextDoor.transform.position - nextDoor.transform.forward;
            GC.ChangeRoom(nextDoor);
        }
    }
}
