using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    public GameObject Light1;
    public Vector2 intensity;
    public GameObject Light2;
    public float offset;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Light1.GetComponent<Light>().intensity = GameController.instance.player.GetComponent<Player>().map(((Mathf.Sin(Time.time) / 2) + 0.5f), 0,1, intensity.x, intensity.y);
        Light2.GetComponent<Light>().intensity = GameController.instance.player.GetComponent<Player>().map(((Mathf.Sin(Time.time * offset) / 2) + 0.5f), 0,1, intensity.x, intensity.y);
	}
}
