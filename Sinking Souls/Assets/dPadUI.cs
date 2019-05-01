using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dPadUI : MonoBehaviour {
    Image image;
    float speed;
    bool display;
    [HideInInspector] public bool onScreen = true;

    private void Start() {
        image = GetComponent<Image>();
    }

    void Update () {
        if (onScreen) {
            if (display) {
                image.color = new Color(image.color.r, image.color.g, image.color.b, (Mathf.Sin(Time.time * speed) / 2) + 0.5f);
            }
            GameObject.Find("MessageController").GetComponent<MessageController>().messageAlphaHolder = GetComponent<Image>().color.a;
        }

    }

    public void apear(float beepSpeed) {
        speed = beepSpeed;
        display = true;
    }

    public void destroy() {
        Destroy(gameObject);
    }

}
