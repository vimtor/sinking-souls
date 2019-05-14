using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnLogic : MonoBehaviour {

    public GameObject parent;
    public Vector3 originalPosition;
    private bool valid = false;
    private bool light = false;
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

        if (transform.parent.gameObject == GameController.instance.currentRoom) return;
        valid = (Random.Range(0, 100) <= GameController.instance.spawnProvabilty);

        if (valid) GameController.instance.activeChests++;
        else {
            light = (Random.Range(0, 100) <= GameController.instance.spawnProvabiltySecondary);
            if (light) {
                GameController.instance.activeSecondaryChests++;
                transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.magenta;
                Debug.Log("Secondary Chest in: " + transform.parent.gameObject.name);
                finalSize *= 0.8f;
            }


        }
    }

    // Update is called once per frame
    void Update () {
        if (valid || light) {
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
            if (light) GetComponent<ChestBehaviour>().LightChest = true;
        }
        
        
	}
}
