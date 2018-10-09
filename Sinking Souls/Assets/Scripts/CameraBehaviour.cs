using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform player;
    public LayerMask mask;
    public int zoom;
    Vector3 center, offset;
    
	void Start () {
        // Setup initial center and offset values,
        // based on how the camera is initially positioned.
        center = new Vector3(0, 0, 0);
        offset = transform.position - center;
        //transform.localPosition.x += zoom;

        SetupCenter(center);
        transform.LookAt(center);
    }
	
	void Update () {
        // SetupCenter(player.position);
        // RotateCamera();
        LookPlayer();
    }

    public void SetupCenter(Vector3 newCenter) {
        center = newCenter;
        transform.position = center + offset;
    }

    private void LookPlayer() {
        transform.LookAt(player.position);
    }

    private void RotateCamera() {
        Vector3 angles = new Vector3(0, InputHandler.RightJoystick.x, 0);
        offset = Quaternion.Euler(angles) * offset;
        transform.position = center + offset;
        transform.LookAt(center);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Wall") {
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Wall") {
            other.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
