using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour {

    public GameObject AttackObject;
    public bool attack;
    private float life;
    public float maxLife;
    public bool dead;
    public bool inactive;
    public float backSpeed;
    public float forwardSpeed;

    public bool orbitAttack = false;
    public bool arrowAttack = false;
    public bool bigAttack = false;

    public bool stopLooking = false;
    public float originalDamage;

    public Vector3 inactivePosition;
    public Vector3 bigInactivePosition = Vector3.zero;
    public Quaternion bigInactiveRotation = Quaternion.identity;
    public Material material;
    private float hitCounter = 0;
    public float hitColorSpeed;
    private float forwardTime=0;


    public float rotationSpeed;
    // Use this for initialization
    void Start () {
        AttackObject = transform.GetChild(0).gameObject;
        stopAttack();
        life = maxLife;
        material = GetComponent<Renderer>().material;
        originalDamage = AttackObject.GetComponent<WeaponHolder>().holder.damage;
	}
	
	void Update () {
        material.SetFloat("_Darken", Mathf.Clamp01(hitCounter));
        if (attack) activateAttack();
        else stopAttack();

        if (life <= 0) dead = inactive = true;



        if (orbitAttack) {
            orbitAttackLogic();
            if (inactive) transform.position = inactivePosition;
        }
        if (dead && bigInactivePosition != new Vector3(0, 0, 0)) {
            transform.position = bigInactivePosition;
            transform.rotation = bigInactiveRotation;
        }
        if (arrowAttack) arrowAttackLogic(forwardTime);

        if (bigAttack) bigAttackLogic();





        if(dead) material.SetFloat("_Darken", 2);
        hitCounter -= Time.deltaTime * hitColorSpeed;
       
	}

    private bool stopRotation = false;
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
        }else 
        if (bigAttack) {
            if (other.tag == "Ground") {
                stopAttack();
                stopRotation = true;
                Debug.Log("Hitted floor");

            }
        }else {
            if (other.tag == "Ground" && dead) {
                bigInactivePosition = transform.position + Vector3.down * 0.4f;
                bigInactiveRotation = transform.rotation;
                Debug.Log("Got One");

            }
        }
    }

    bool selected = false;
    public void giantAttack(bool me) {
        bigAttack = true;
        selected = me;
    }
    private float stopedCounter = 0;
    private void bigAttackLogic() {
        if (selected && !stopRotation) {
            transform.Rotate(new Vector3(1, 0, 0), Time.deltaTime * rotationSpeed);
            activateAttack();
        }
        else {
            if(stopedCounter > 2) {
                inactive = true;

            }
            stopedCounter += Time.deltaTime;
        }
    }

    [HideInInspector]public float orbitAttackCounter = 0;
    public float initialTime;

    public void launch() {
        orbitAttack = true;
    }

    Vector3 playerDir;
    public float forwardDistLaunch;

    private void orbitAttackLogic() {
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

    float speedMultiplier;
    public void forwadLaunch(float time, float speedMult) {
        arrowAttack = true;
        forwardTime = time;
        speedMultiplier = speedMult;
    }

    private float forwardLaunchCounter = 0;

    private void arrowAttackLogic(float time) {
        if(forwardLaunchCounter < time) {
            GetComponent<Rigidbody>().velocity = transform.forward * forwardSpeed * speedMultiplier;
            activateAttack();
        }
        else {
            if(forwardLaunchCounter < time + 0.5) {
                GetComponent<Rigidbody>().velocity = Vector3.Lerp(transform.forward, Vector3.up, Mathf.Clamp01(((forwardLaunchCounter - time) * 2) * 2)) * forwardSpeed * speedMultiplier;
                transform.forward = Vector3.Lerp(transform.forward, Vector3.up, Mathf.Clamp01((forwardLaunchCounter - time) * 2));
            }
            else {
                stopAttack();
                inactive = true;
            }
        }
        forwardLaunchCounter += Time.deltaTime;
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
        bigAttack = false;
        arrowAttack = false;
        stopLooking = false;
        orbitAttackCounter = 0;
        forwardLaunchCounter = 0;
        stopedCounter = 0;
        stopRotation = false;
        bigInactivePosition = Vector3.zero;
        AttackObject.GetComponent<WeaponHolder>().holder.damage = originalDamage;
    }
}
