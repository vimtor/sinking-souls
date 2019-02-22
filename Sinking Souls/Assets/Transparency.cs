using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {

    public List<GameObject> objects;
    private float fadeD = 4;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 player = GameController.instance.player.transform.position + new Vector3(0,1,0);
        Vector3 camera = CameraManager.instance.virtualCamera.transform.position;
        Vector3 pC = camera - player;
        Debug.DrawRay(camera,player - camera, Color.red);
        foreach (GameObject go in objects) {
            go.GetComponent<Renderer>().material.SetVector("_PlayerPosition", player + pC.normalized * 2);
            go.GetComponent<Renderer>().material.SetVector("_Camera", camera);
            go.GetComponent<Renderer>().material.SetFloat("_limit", fadeD);
        }
	}
}
