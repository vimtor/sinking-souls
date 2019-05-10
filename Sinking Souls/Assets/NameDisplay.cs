using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameDisplay : MonoBehaviour {
    TextMeshPro text;
    float apearingSpeed = 0.7f;
	// Use this for initialization
	void Start () {
        text = GetComponent<TextMeshPro>();
        text.color = new Vector4(text.color.r, text.color.g, text.color.b, 0);
    }
    private bool show;
	// Update is called once per frame
	void Update () {
        if (!show) {
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - apearingSpeed * Time.deltaTime);
            if (text.color.a <= 0) text.color = new Vector4(text.color.r, text.color.g, text.color.b, 0);

        }
        else {
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a + apearingSpeed * Time.deltaTime);
            if (text.color.a >= 1) text.color = new Vector4(text.color.r, text.color.g, text.color.b, 1);
        }
    }
    public void Show() {
        show = true;
    }
    public void Hide() {
        show = false;
    }
}
