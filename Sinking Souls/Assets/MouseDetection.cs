using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseDetection : MonoBehaviour {

    public bool on = false;
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
      
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("premio");
    }

    public void OnMouseEnter()
    {
        Debug.Log("Testing");
    }

}
