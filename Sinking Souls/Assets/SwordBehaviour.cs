using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour {

    GameObject AttackObject;
    public bool attack;
    private float life;
    public float maxLife;
    public bool dead;
    public bool inactive;
    public float backSpeed;
    public float forwardSpeed;

    public bool orbitAttack = false;
    public bool stopLooking = false;

    public Vector3 inactivePosition;
    public Material material;
    private float hitCounter = 0;
    public float hitColorSpeed;
    // Use this for initialization
    void Start () {
        AttackObject = transform.GetChild(0).gameObject;
        stopAttack();
        life = maxLife;
        material = GetComponent<Renderer>().material;
	}
	
	void Update () {
        material.SetFloat("_Darken", Mathf.Clamp01(hitCounter));
        if (attack) activateAttack();
        else stopAttack();

        if (life <= 0) dead = inactive = true;

        if (orbitAttack) orbitAttackLogic();

        if (inactive) transform.position = inactivePosition;

        if(dead) material.SetFloat("_Darken", 2);
        hitCounter -= Time.deltaTime * hitColorSpeed;
       
	}


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Weapon") {
            life -= other.GetComponent<WeaponHolder>().holder.damage;
            hitCounter = 1;
            Debug.Log("Hit");
        }
        if (orbitAttack) {
            if(other.tag == "Ground" || other.tag == "Obstacle") {
                stopAttack();
                inactive = true;
                inactivePosition = transform.position + transform.forward * 0.7f;
            }
        }
    }

    [HideInInspector]public float orbitAttackCounter = 0;
    public float initialTime;

    public void launch() {
        orbitAttack = true;
    }
    Vector3 playerDir;
    public float forwardDistLaunch;
    public void orbitAttackLogic() {
        if (!inactive) {
            if (orbitAttackCounter < initialTime) {
                transform.position -= transform.forward * Time.deltaTime * backSpeed;
                playerDir = ((GameController.instance.player.transform.position + Vector3.up +(GameController.instance.player.GetComponent<Rigidbody>().velocity * Time.deltaTime * forwardDistLaunch)) - transform.position).normalized;
            }
            else {
                transform.forward = playerDir;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                stopLooking = true;
                activateAttack();
                transform.position += playerDir * Time.deltaTime * forwardSpeed;
            }

            orbitAttackCounter += Time.deltaTime;
        }
    }

    public void lookPlayer() {
        transform.forward = ((GameController.instance.player.transform.position + Vector3.up) - transform.position).normalized;
    }

    public void revive() {
        life = maxLife;
        dead = inactive = false;
    }

    public void activateAttack() {
        AttackObject.SetActive(true);
    }
    public void stopAttack() {
        AttackObject.SetActive(false);

    }
    public void resetSword() {

        inactive = false;
        orbitAttack = false;
        stopLooking = false;
        orbitAttackCounter = 0;
    }
}
