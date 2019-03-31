using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Renderer>().material.SetVector("_PlayerPosition", GameController.instance.player.transform.position);
        gameObject.transform.forward = new Vector3((GameObject.Find("Game Camera").transform.position - transform.position).x, 0, (GameObject.Find("Game Camera").transform.position - transform.position).z);

    }
}
