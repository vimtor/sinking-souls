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
        switch (other.tag) {
            case "Obstacle":
            case "Wall":
                SetAlpha(other.GetComponent<Renderer>().material, 0.35f);
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch (other.tag) {
            case "Obstacle":
            case "Wall":
                SetAlpha(other.GetComponent<Renderer>().material, 1f);
                break;

            default:
                break;
        }

    }

    private void SetAlpha(Material material, float newAlpha) {
        Color newColor = material.color;
        newColor.a = newAlpha;

        material.color = newColor;
    }
}
