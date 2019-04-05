using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoockCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.forward = -1 * new Vector3((GameObject.Find("Game Camera").transform.position - transform.position).x, (GameObject.Find("Game Camera").transform.position - transform.position).y, (GameObject.Find("Game Camera").transform.position - transform.position).z);

    }
}
