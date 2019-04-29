using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnLogic : MonoBehaviour {

    public GameObject parent;
    public Vector3 originalPosition;
    private bool valid = false;
    private bool spawn = false;

    public float startSize = 0;
    public float finalSize;
    public float growingSpeed = 10;
    public float acceleration = 100;
    //public float rotatingSpeed;
    // Use this for initialization
    void Start () {
      
        parent = transform.parent.gameObject;
        originalPosition = transform.position;
        transform.position = originalPosition + Vector3.down * 10000;
        
        finalSize = transform.localScale.x;
        transform.localScale = new Vector3(1, 1, 1) * startSize;


        if(Random.Range(0,100) <= GameController.instance.spawnProvabilty) {
            if(!(GameController.instance.activeChests >= GameController.instance.maximumPerLevel)) {
                valid = true;
                Debug.Log("random and less than maximum");
            }

        }
        else {
            valid = false;
            if (GameController.instance.activeChests < GameController.instance.minimumPerLevel) {
                valid = true;
                Debug.Log("not random and less than minimum");

            }
        }

        if (GameController.instance.currentRoom == transform.parent.gameObject) {
            if (GameController.instance.roomEnemies.Count == 0) {
                valid = false;
                Debug.Log("DISCARTED");

            }
        }
        if (valid) GameController.instance.activeChests++;
    }

    // Update is called once per frame
    void Update () {
        if (valid) {
            if (GameController.instance.currentRoom == transform.parent.gameObject) {
                if (GameController.instance.roomEnemies.Count == 0) {
                    spawn = true;
                    transform.position = originalPosition;
                }
            }

            if (spawn) {
                if(transform.localScale.x < finalSize) {
                    transform.localScale += new Vector3(1, 1, 1) * growingSpeed * Time.deltaTime;
                    growingSpeed += acceleration * Time.deltaTime;
                }
                else {
                    transform.localScale = new Vector3(1,1,1) * finalSize;
                }
            }


        }
	}
}
