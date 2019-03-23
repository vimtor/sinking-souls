using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscilationAtHit : MonoBehaviour {
    public bool oscilating = false;
    public float oscilationTime;
    public float amount;
    public float speed;
    private float time = 0;
    private Vector3 originalScale;
    private void Start() {
        originalScale = transform.localScale;
    }

    private void Update() {
        if (time > oscilationTime) oscilating = false;
        if (oscilating) {
            transform.localScale = originalScale + new Vector3(0, Mathf.Sin(Time.time * speed) * amount,0);
        }
        time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (!oscilating) {
            switch (other.tag) {
                case "Weapon":
                oscilating = true;
                time = 0;
                break;

                case "Ability":
                oscilating = true;
                time = 0;
                break;
            }
        }


    }
}
