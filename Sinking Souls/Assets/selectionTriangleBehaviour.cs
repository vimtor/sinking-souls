using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionTriangleBehaviour : MonoBehaviour {

    public float floatingSpeed;
    public float floatingAmount;
    public float rotationSpeed;
    public float height;
    public Color maxHealth;
    public Color midHealth;
    public Color nearDead;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        GetComponent<MeshRenderer>().enabled = true;


        

        if (GameController.instance.player.GetComponent<Player>().lockedEnemy != null) {

            transform.position = GameController.instance.player.GetComponent<Player>().lockedEnemy.transform.Find("SelectedPoint").transform.position + (Vector3.up * (height + (Mathf.Sin(Time.time * floatingSpeed) * floatingAmount)));
            transform.Rotate(new Vector3(0,0,1), rotationSpeed);
            if(GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Entity>().Health >= GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Entity>().MaxHealth / 2f)
            {
                //Green to Orange (100 to 50%)
                GetComponent<Renderer>().material.color = Color.Lerp(maxHealth, midHealth, GameController.instance.player.GetComponent<Player>().map(GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Entity>().Health, GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Enemy>().MaxHealth/2f, GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Enemy>().MaxHealth, 1, 0));
            }
            else{
                //Orange to Red (50 to 0%)
                GetComponent<Renderer>().material.color = Color.Lerp(midHealth, nearDead, GameController.instance.player.GetComponent<Player>().map(GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Entity>().Health, 0, GameController.instance.player.GetComponent<Player>().lockedEnemy.GetComponent<Enemy>().MaxHealth/2f, 1, 0));
            }
        }
        else {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
