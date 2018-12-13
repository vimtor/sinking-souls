using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float health;
    public float maxHealth;
    public float walkSpeed;
    public Weapon weapon;
    public GameObject hand;
    public Modifier baseModifier;
    public GameObject hitParticle;
    public GameObject healParticles;

    [HideInInspector] public bool thrown;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 facingDir;
    [HideInInspector] public bool hit;
    [HideInInspector] new public CapsuleCollider collider;
    [HideInInspector] public enum ModifierState {FIRE, TOXIC, ELECTRIC, ICE};
    [HideInInspector] public Dictionary<ModifierState, int> currentModifierState = new Dictionary<ModifierState, int>();
    [HideInInspector] public Color originalColor;
    [HideInInspector] public bool gettingDamage = false;


    protected void OnStart() {
        originalColor = transform.GetChild(1).GetComponent<Renderer>().material.color;

        for (int i = 0; i < 4; i++) {
            currentModifierState[(ModifierState)i] = 0;
        }
        
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingDir = Vector3.zero;
        collider = GetComponent<CapsuleCollider>();
        hit = false;
    }

    protected void Apply(Modifier modifier) {
        if(modifier != null) {
            modifier.Apply(this.gameObject);
        }
    }

    public void EquipWeapon() {
        weapon.Instantiate(hand, this.gameObject);
    }

    public void TakeDamage(float damage) {
        health -= damage;
        transform.GetChild(1).GetComponent<Renderer>().material.color = Color.red;
        gettingDamage = true;
        GameController.instance.StartCoroutine(ResetColor(0.1f));
    }

    public void Heal(float heal) {
        health += health + heal > maxHealth ? maxHealth : heal;

        bool noHeal = true;
        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).tag == "FxTemporaire") noHeal = false;
        }

        if (noHeal) {
            GameObject healGo = Instantiate(healParticles);
            healGo.transform.position = gameObject.transform.position;
            healGo.transform.parent = gameObject.transform;
        }
    }

    public void Heal() {
        Heal(maxHealth);
    }

    IEnumerator ResetColor(float time) {
        yield return new WaitForSeconds(time);

        try
        {
            gettingDamage = false;
            transform.GetChild(1).GetComponent<Renderer>().material.color = originalColor;
        }
        catch(Exception exception)
        {
            yield break;
        }
        
    }


    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Weapon" && other.GetComponent<WeaponHolder>().owner.tag != tag) {
            if (other.GetComponent<WeaponHolder>().holder.hitting && !hit) {
                hit = true;
                TakeDamage(other.GetComponent<WeaponHolder>().holder.damage);
                Apply(other.GetComponent<WeaponHolder>().holder.modifier);
                
                if(tag == "Enemy") CameraManager.instance.Hit(0.05f, 2.5f);
                GameObject hitO = Instantiate(hitParticle);
                hitO.transform.position = other.transform.position;
                Destroy(hitO, 1);
            }
        }
        else if (other.tag == "Ability") {
            if(gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) {
                TakeDamage(other.GetComponent<AbilityHolder>().holder.damage);
                Apply(other.gameObject.GetComponent<AbilityHolder>().holder.modifier);
            }
        }
    }


    private void OnTriggerExit(Collider other) {

        if (other.tag == "Weapon") {
            hit = false;
        }
        else if (other.tag == "Ability") {
            if (gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) {
                hit = false;
            }
        }

    }
}
