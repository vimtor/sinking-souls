using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCloth : MonoBehaviour {
    public float deathDuration = 1;
	// Use this for initialization
	void Start () {
		
	}
    float t = 0;
	// Update is called once per frame
	void Update () {
        GetComponent<Renderer>().material.SetFloat("_height", GameController.instance.player.GetComponent<Player>().map(t, 0, deathDuration, 1, 0));
        t += Time.deltaTime;
        if(GameController.instance.player.GetComponent<Player>().map(t, 0, deathDuration, 1, 0) <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
