using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour {

    GameObject AttackObject;
    public bool attack;
    private float life;
    public float maxLife;
    public bool dead;

	// Use this for initialization
	void Start () {
        AttackObject = transform.GetChild(0).gameObject;
        stopAttack();
        life = maxLife;
	}
	
	void Update () {
        if (attack) activateAttack();
        else stopAttack();

        if (life <= 0) dead = true;
       
	}


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Weapon") {
            life -= other.GetComponent<WeaponHolder>().holder.damage;
        }
    }

    public void lookPlayer() {
        transform.forward = ((GameController.instance.player.transform.position + Vector3.up) - transform.position).normalized;
    }

    public void revive() {
        life = maxLife;
    }

    public void activateAttack() {
        AttackObject.SetActive(true);
    }
    public void stopAttack() {
        AttackObject.SetActive(false);

    }
}
