using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour {

    public static CameraManager instance = null; // Singleton.

    [HideInInspector] public Transform player;

    private CinemachineVirtualCamera virtualCamera;

    private Vector3 center, offset;
    private BoxCollider boxCollider;
    private bool gameOn = false;

    private void Awake() {

        #region SINGLETON
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
        #endregion

        virtualCamera = transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
    }


    /// <summary>
    /// Setup initial center and offset values, based on how the camera is initially positioned.
    /// </summary>
    /// <param name="roomCenter"></param>
    public void SetupCamera (Vector3 roomCenter) {
        center = roomCenter;
        offset = new Vector3(-21, 17, -21);
        boxCollider = GetComponent<BoxCollider>();

        SetupCenter(center);
        transform.LookAt(center);
        gameOn = true;

        //virtualCamera.LookAt = player;
        virtualCamera.Follow = player.transform.Find("CameraFollow");
    }
	
	void Update () {
        //if (gameOn) {
        //    SetupCenter(player.transform.position);
        //    MoveCollider();
        //}
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

    #region SHAKE

    public void Shake(float duration, float magnitude) {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    public IEnumerator ShakeCoroutine(float duration, float magnitude) {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            float x = transform.position.x + Random.Range(-1f, 1f) * magnitude;
            float y = transform.position.y + Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, orignalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = orignalPosition;
    }

    #endregion

    #region HIT

    public void Hit(float duration, float magnitude) {
        StartCoroutine(HitCoroutine(duration, magnitude));
    }

    public IEnumerator HitCoroutine(float duration, float magnitude) {

        float originalSize = GetComponent<Camera>().orthographicSize;

        for(float t = 0.0f; t <= duration; t += Time.deltaTime) {
            float newSize = Mathf.Lerp(originalSize, originalSize - magnitude, Mathf.PingPong(t, duration / 2));
            GetComponent<Camera>().orthographicSize = newSize;

            yield return null;
        }

        GetComponent<Camera>().orthographicSize = originalSize;

    }

    #endregion

    #region ROTATION

    private void LookPlayer() {
        transform.LookAt(player.position);
    }

    private void RotateCamera() {
        Vector3 angles = new Vector3(0, InputHandler.RightJoystick.x, 0);
        offset = Quaternion.Euler(angles) * offset;
        transform.position = center + offset;
        transform.LookAt(center);
    }

    #endregion

    #region ALPHA
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

    public IEnumerator Transition(Vector3 targetPosition, float delay = 1.0f) {

        Vector3 startingPosition = transform.position;

        for (float t = 0.0f; t <= 1.0f; t += Time.deltaTime / delay) {
            SetupCenter(Vector3.Lerp(startingPosition, targetPosition, t));
            yield return null;
        }

    }

    #endregion

    #region COLLISION

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

    #endregion


}
