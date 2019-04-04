using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    [HideInInspector]
    public bool locked = false;

    [HideInInspector]
    public GameObject nextDoor;

    private RaycastHit hit;

    void Start () {
        SetNext();
    }

    void SetNext() {
        int layerMask = 1 << 13 | 1<<18;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
            nextDoor = hit.transform.gameObject;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player" && !locked) {
            other.GetComponent<Transform>().position = nextDoor.transform.position - nextDoor.transform.forward*2.1f - new Vector3(0, 2.5f, 0);
            GameController.instance.ChangeRoom(nextDoor);
        }
    }
}
