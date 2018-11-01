using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIShowLife : MonoBehaviour {

    public GameObject enemy;
    public GameObject player;
    public Text playerLife;
    public Text enemyLife;

	// Use this for initialization
	void Start () {
        playerLife.text = "Player: " + player.GetComponent<Player>().health;
        enemyLife.text = "Enemy: " + enemy.GetComponent<Enemy>().health;
	}
	
	// Update is called once per frame
	void Update () {
        playerLife.text = "Player: " + player.GetComponent<Player>().health;
        enemyLife.text = "Enemy: " + enemy.GetComponent<Enemy>().health;
    }
}
