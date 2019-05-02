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
        }
        if (fadeOut) {
            GetComponent<Image>().color = new Color(0, 0, 0, GetComponent<Image>().color.a + speed * Time.unscaledDeltaTime);
            Debug.Log("Fade " + GetComponent<Image>().color.a);

        }
    }
    public void FadeIn(float s) {
        fadeIn = true;
        fadeOut = false;

        speed = 1/s;
    }
    public void FadeOut(float s) {
        Debug.Log("Fade Out Activated");
        fadeIn = false;
        fadeOut = true;
        speed = 1/s;
    }
}
