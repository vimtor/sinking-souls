using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallBehaviour : MonoBehaviour {

    public float lifeSpan;
    public float speed;
    public GameObject particles;
    private bool grown = false;
    [HideInInspector] public GameObject GrowingPosition;

    [Header("Lights")]
    public GameObject blue;
    public GameObject white;
    public float wobbleSpeed;

    [Header("Growing and shrinking speeds")]
    public Vector2 sizingSpeeds;
    [Header("Min and max sizes")]
    public Vector2 sizes;

    private float time;

    // Use this for initialization
    void Start () {
        gameObject.transform.localScale = new Vector3(sizes.x, sizes.x, sizes.x);
        Destroy(gameObject, lifeSpan);
	}

    // Update is called once per frame
    void Update() {
        //blue.GetComponent<Light>().intensity = (((Mathf.Sin(Time.time * wobbleSpeed) / 2) + 0.5f) * 0.4f) + 0.6f;

        StartCoroutine(whiteLight(0));

        if (!grown && gameObject.transform.localScale.x < sizes.y) {
            gameObject.transform.localScale += Vector3.one * sizingSpeeds.x * Time.deltaTime;
            transform.position = GrowingPosition.transform.position;
        }
        if (gameObject.transform.localScale.x >= sizes.y && grown == false) {
            grown = true;
            GetComponent<Rigidbody>().velocity = speed * transform.forward.normalized;
        }
        if (grown == true) {
            gameObject.transform.localScale -= Vector3.one * sizingSpeeds.y * Time.deltaTime;
            if(gameObject.transform.localScale.x <= 0) Destroy(gameObject);
        }

        time += Time.deltaTime;

    }

    IEnumerator whiteLight(float t) {
        yield return new WaitForSeconds(t);
        white.SetActive(true);
        StartCoroutine(whiteLightOff(0.1f));
    }
    IEnumerator whiteLightOff(float t) {
        yield return new WaitForSeconds(t);
        white.SetActive(false);
        StartCoroutine(whiteLight(Random.Range(0.5f,1f)));
    }

}
