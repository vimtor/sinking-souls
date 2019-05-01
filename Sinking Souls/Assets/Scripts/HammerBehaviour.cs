using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBehaviour : MonoBehaviour {
    private bool strongAttack;
    private bool noAttack;

    public float repulsionTime;
    public float repulsionForce;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        noAttack = false;
        if (transform.GetComponentInParent<AIController>().CurrentState.name.Contains("Strong")) {
            strongAttack = true;
        }
        else if (transform.GetComponentInParent<AIController>().CurrentState.name.Contains("Light")) {
            strongAttack = false;

        }
        else noAttack = true;
	}

    private void OnTriggerEnter(Collider other) {
        if (!noAttack) {
            if(other.gameObject.tag == "Player") {
                if (strongAttack) {

                }
                else {
                    other.gameObject.GetComponent<Player>().m_PlayerState = Player.PlayerState.REPULSED;
                    other.gameObject.GetComponent<Player>().repulsionForce = repulsionForce;
                    Vector3 dir = (GameController.instance.player.transform.position - transform.position).normalized;
                    dir.y = 0;
                    other.gameObject.GetComponent<Player>().repulsionVector =  dir;
                    other.gameObject.GetComponent<Player>().repulsionTime = repulsionTime;
                }
            }
        }
    }
}
