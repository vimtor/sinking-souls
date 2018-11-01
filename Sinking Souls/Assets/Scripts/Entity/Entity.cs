using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float health;
    public float walkSpeed;
    public Weapon weapon;
    public GameObject hand;
    public Modifier baseModifier;

    [HideInInspector] public bool thrown;
    [HideInInspector] public Rigidbody rb;
    /*[HideInInspector]*/ public Animator animator;
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

    protected void Apply(Modifier modifier) {
        if(modifier != null) {
            foreach (Effect effect in modifier.effects) {
                effect.Apply(gameObject);
            }
        }
        
    }

    public void EquipWeapon() {
        weapon.Instantiate(hand);
    }

    protected void TakeDamage(float damage) {
        health -= damage;
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Weapon") {
            if (other.GetComponent<WeaponHolder>().holder.hitting && !hit) {
                hit = true;
                TakeDamage(other.GetComponent<WeaponHolder>().holder.Damage);
                Apply(other.GetComponent<WeaponHolder>().holder.modifier);
                
                if(tag == "Enemy") CameraManager.instance.Hit(0.05f, 2.5f);
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
