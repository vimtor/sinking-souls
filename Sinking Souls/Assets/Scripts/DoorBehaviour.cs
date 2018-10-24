using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    [HideInInspector]
    public bool locked = false;

    [HideInInspector]
    public GameObject nextDoor;

    void Start () {
        SetNext();
    }

    void SetNext() {
        int layerMask = 1 << 13;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            nextDoor = hit.transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !locked) {
            Debug.Log("TP");
            other.GetComponent<Transform>().position = nextDoor.transform.position - nextDoor.transform.forward;
            GameController.instance.ChangeRoom(nextDoor);
        }
    }
}
