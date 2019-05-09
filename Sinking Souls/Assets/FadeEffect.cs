using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour {

    private bool fadeIn = false;
    private bool fadeOut = false;
    private float speed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (fadeIn) {
            GetComponent<Image>().color = new Color(0, 0, 0, GetComponent<Image>().color.a - speed * Time.unscaledDeltaTime);
            if (GetComponent<Image>().color.a <= 0) fadeIn = false;

        }
        if (fadeOut) {
            GetComponent<Image>().color = new Color(0, 0, 0, GetComponent<Image>().color.a + speed * Time.unscaledDeltaTime);
            if (GetComponent<Image>().color.a >= 1) fadeOut = false;
        }
    }
    public void FadeIn(float s) {
        if (fadeIn) {
            return;
        }

        fadeIn = true;
        fadeOut = false;
        GetComponent<Image>().color = new Color(0, 0, 0, 1);
        speed = 1/s;
    }

    public void FadeOut(float s) {
        if ( fadeOut) {
            return;
        }
        if (GetComponent<Image>().color.a >= 1) return;
        GetComponent<Image>().color = new Color(0, 0, 0, 0);

        fadeIn = false;
        fadeOut = true;
        speed = 1/s;
    }
}
