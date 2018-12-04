using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryChangeColor : MonoBehaviour {

    GameObject child;
    Color colorStart = Color.red;
    Color colorEnd = Color.green;
    float duration = 1.0f;
    // Use this for initialization
    void Start () {
        child = gameObject.transform.GetChild(1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        child.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, lerp);
	}
}
