using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform player;

    private Vector3 center, offset;
    private BoxCollider boxCollider;
    private bool gameOn = false;

    public void SetupCamera (Vector3 roomCenter) {
        // Setup initial center and offset values,
        // based on how the camera is initially positioned.
        center = roomCenter;
        offset = new Vector3(-21.0f, 17, -21);
        boxCollider =  GetComponent<BoxCollider>();

        SetupCenter(center);
        transform.LookAt(center);
        gameOn = true;
    }
	
	void Update () {
        if (gameOn) {
            LookPlayer();
            MoveCollider();
        }
    }

    private void MoveCollider() {
        Vector3 size = boxCollider.size;
        Vector3 center = boxCollider.center;

        size.z = Vector3.Distance(transform.position, player.position);
        center = (transform.position + player.position) / 2;
        
        boxCollider.center = boxCollider.transform.InverseTransformPoint(center);
        boxCollider.size = size;
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

    private IEnumerator FadeAlpha(GameObject other, float newAlpha, float delay = 1.0f) {

        Material material = other.GetComponent<Renderer>().material;

        Color newColor = material.color;
        float alpha = newColor.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / delay) {
            newColor.a = Mathf.Lerp(alpha, newAlpha, t);
            material.color = newColor;
            yield return null;
        }

    }

    public IEnumerator Transition(Vector3 targetPosition, float delay = 1.0f) {

        Vector3 startingPosition = transform.position;

        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / delay) {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
            yield return null;
        }

    }

    private void SetAlpha(Material material, float newAlpha) {
        Color newColor = material.color;
        newColor.a = newAlpha;

        material.color = newColor;
    }

    private void OnTriggerEnter(Collider other) {

        switch (other.tag) {
            case "Obstacle":
            case "Wall":
                StartCoroutine(FadeAlpha(other.gameObject, 0.20f));    
                break;

            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        switch (other.tag) {
            case "Obstacle":
            case "Wall":
                StartCoroutine(FadeAlpha(other.gameObject, 1f));
                break;

            default:
                break;
        }

    }


}
