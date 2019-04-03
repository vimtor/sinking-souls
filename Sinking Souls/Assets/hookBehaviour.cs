using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hookBehaviour : MonoBehaviour {

    public bool move = false;
    public float movementSpeed;
    public GameObject target;
    public GameObject player;

    void Start() {
        transform.position = player.transform.position + new Vector3(0,1,0);
    }

    // Update is called once per frame
    void Update() {
        if (move) {
            player.GetComponent<Player>().m_PlayerState = Player.PlayerState.PULLING;
            if (target != null && Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.transform.position.x, target.transform.position.z)) > 0.2f) {
                float step = movementSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
            }
            else {
                move = false;
                player.GetComponent<Hook>().move = true;
            }     
        }
    }
}