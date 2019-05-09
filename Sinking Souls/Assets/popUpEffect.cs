using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class popUpEffect : MonoBehaviour {
    bool isImage;
    Image image;
    TextMeshPro text;
    public float apearingSpeed = 30;
    bool fadeOut = false;
	// Use this for initialization
	void Start () {
        if (GetComponent<Image>()) {
            image = GetComponent<Image>();
            isImage = true;
        }
        else {
            text = GetComponent<TextMeshPro>();
            isImage = false;
        }
        if(isImage)
        image.color = image.color * new Vector4(1, 1, 1, 0);
        else {
            text.color = text.color * new Vector4(1, 1, 1, 0);

        }
    }
	
	// Update is called once per frame
	void Update () {
        if (isImage) {
            if (fadeOut) {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a - apearingSpeed * Time.deltaTime);
                if (image.color.a <= 0) Destroy(gameObject);
            }
            else {
                image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a + apearingSpeed * Time.deltaTime);

            }
        }
        else {
            if (fadeOut) {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - apearingSpeed * Time.deltaTime);
                if (text.color.a <= 0) Destroy(gameObject);
            }
            else {
                text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a + apearingSpeed * Time.deltaTime);

            }
        }

    }

    public void destroy() {
        fadeOut = true;
    }
}
