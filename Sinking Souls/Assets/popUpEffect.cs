using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popUpEffect : MonoBehaviour {
    Image image;
    public float apearingSpeed = 30;
    bool fadeOut = false;
	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
        image.color = image.color * new Vector4(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeOut) {
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a - apearingSpeed * Time.deltaTime);
            if (image.color.a <= 0) Destroy(gameObject);
        }
        else {
            image.color = new Vector4(image.color.r, image.color.g, image.color.b, image.color.a + apearingSpeed * Time.deltaTime);

        }
    }

    public void destroy() {
        fadeOut = true;
    }
}
