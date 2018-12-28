using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tryChangeColor : MonoBehaviour {

    GameObject child;
    Color colorStart = Color.red;
    Color colorEnd = new Color( 0, 0.5f, 0, 1 );
    float duration = 1.0f;

    // Use this for initialization
    void Start () {
        child = gameObject.transform.GetChild(1).gameObject;
        if(gameObject.GetComponent<Entity>()) {
            colorStart = GetComponent<Entity>().originalColor;
            colorStart.r += 0.2f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!gameObject.GetComponent<Entity>().gettingDamage) {
            float lerp = Mathf.PingPong(Time.time, duration) / duration;
            child.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, lerp);
        }
	}
}
