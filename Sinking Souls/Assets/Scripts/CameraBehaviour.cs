using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    public Transform player;
    Vector3 center, offset;
    
    new private BoxCollider collider;

    void Start () {
        // Setup initial center and offset values,
        // based on how the camera is initially positioned.
        center = new Vector3(0, 0, 0);
        offset = transform.position - center;
        collider =  GetComponent<BoxCollider>();

        SetupCenter(center);
        transform.LookAt(center);
    }
	
	void Update () {
        // SetupCenter(player.position);
        // RotateCamera();
        LookPlayer();
        MoveCollider();
        
    }

    private void MoveCollider() {
        Vector3 size = collider.size;
        Vector3 center = collider.center;

        size.z = Vector3.Distance(transform.position, player.position);
        center = (transform.position + player.position) / 2;
        
        collider.center = collider.transform.InverseTransformPoint(center);
        collider.size = size;

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
                StartCoroutine(FadeAlpha(other.gameObject, 0.35f));
                    
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

    private void SetAlpha(Material material, float newAlpha) {
        Color newColor = material.color;
        newColor.a = newAlpha;

        material.color = newColor;
    }


    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube((transform.localPosition + player.position) / 2, new Vector3(1, 1, 1));
        Debug.Log((transform.localPosition + player.position) / 2);
    }
}
