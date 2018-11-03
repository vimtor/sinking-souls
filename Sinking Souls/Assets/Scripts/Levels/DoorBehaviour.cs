using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour {

    [HideInInspector]
    public bool locked = false;

    [HideInInspector]
    public GameObject nextDoor;

    private RaycastHit hit;

    void Start () {
        SetNext();
    }

    private void OnDrawGizmos() {
        if (GameController.instance.debugMode) {
            Gizmos.color = Color.red;
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance);
            Gizmos.DrawRay(ray);
        }
        
    }

    void SetNext() {
        int layerMask = 1 << 13;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
            nextDoor = hit.transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !locked) {
            other.GetComponent<Transform>().position = nextDoor.transform.position - nextDoor.transform.forward - new Vector3(0, 2.5f, 0);
            GameController.instance.ChangeRoom(nextDoor);
        }
    }
}
