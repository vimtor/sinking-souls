using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionTriangleBehaviour : MonoBehaviour {

    public float floatingSpeed;
    public float floatingAmount;
    public float rotationSpeed;
    public float height;
    public Color health;
    public Color nearDead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<MeshRenderer>().enabled = true;

        if (GameController.instance.player.GetComponent<Player>().lockedEnemy != null) {
            transform.position = GameController.instance.player.GetComponent<Player>().lockedEnemy.transform.position + Vector3.up * ((height * Mathf.Clamp(GameController.instance.player.GetComponent<Player>().lockedEnemy.transform.localScale.y - 0.2f,1.2f,10)) + Mathf.Sin(Time.time * floatingSpeed) * floatingAmount);
            transform.Rotate(new Vector3(0,0,1), rotationSpeed);
            GetComponent<Renderer>().material.color = Color.Lerp(health, nearDead, GameController.instance.player.GetComponent<Player>().map(GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Entity>().Health, 0, GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Enemy>().MaxHealth, 1, 0));
        }
        else {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
