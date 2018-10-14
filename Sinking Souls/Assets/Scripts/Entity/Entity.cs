using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float health;
    public float walkSpeed;
    public Weapon weapon;
    public GameObject hand;
    public Modifier baseModifier;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 facingDir;
    /*[HideInInspector]*/ public bool hit;
    [HideInInspector] new public CapsuleCollider collider;

    protected void OnStart() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingDir = Vector3.zero;
        collider = GetComponent<CapsuleCollider>();
        hit = false;
    }

    protected void Apply(Modifier modifier)  {
        List<Effect> effects = modifier.effects;
        foreach(Effect effect in effects) {
            effect.Apply(gameObject);
        }
    }

    public void EquipWeapon() {
        weapon.Instantiate(hand);
    }

    protected void TakeDamage(float damage) {
        health -= damage;
        //Debug.Log(health);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Weapon") {
            if (other.GetComponent<WeaponHolder>().holder.hitting) {
                hit = true;
                Debug.Log("Sword hitted me");
                TakeDamage(other.GetComponent<WeaponHolder>().holder.Damage);
                Apply(other.GetComponent<WeaponHolder>().holder.modifier);
            }
        }
        else if (other.tag == "Ability") {
            if(gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) {
                hit = true;
                TakeDamage(other.GetComponent<AbilityHolder>().holder.Damage);
                Apply(other.gameObject.GetComponent<Ability>().modifier);
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
