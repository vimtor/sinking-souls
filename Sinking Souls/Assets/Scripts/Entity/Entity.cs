using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    public float health;
    public float walkSpeed;
    public WeaponSO weapon;
    public GameObject hand;
    public ModifierSO baseModifier;

    protected Rigidbody rb;
    protected Animator animator;
    public Vector3 facingDir;
    new protected CapsuleCollider collider;

    protected void OnStart() {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        facingDir = Vector3.zero;
        collider = GetComponent<CapsuleCollider>();
    }

    protected void Apply(ModifierSO modifier)  {
        List<EffectSO> effects = modifier.effects;
        foreach(EffectSO effect in effects) {
            effect.Apply(gameObject);
        }
    }

    public void EquipWeapon() {
        weapon.Instantiate(hand);
    }

    protected void TakeDamage(float damage) {
        health -= damage;
        Debug.Log(health);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Weapon") {
            if (other.GetComponent<WeaponHolder>().holder.hitting) { 
                Debug.Log("Sword hitted me");
                TakeDamage(other.GetComponent<WeaponHolder>().holder.Damage);
                Apply(other.GetComponent<WeaponHolder>().holder.modifier);
            }
        }
        else if (other.tag == "Ability") {
            if(gameObject.tag == other.GetComponent<AbilityHolder>().holder.target) { 
                TakeDamage(other.GetComponent<AbilityHolder>().holder.Damage);
                Apply(other.gameObject.GetComponent<AbilitySO>().modifier);
            }
        }
    }
}
