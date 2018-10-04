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
    protected Vector3 facingDir;
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
    }

    protected void OnCollisionEnter(Collision collision) {
        Debug.Log(collision.collider.name);


        if (collision.collider.tag == "Weapon") {
            Apply(collision.collider.gameObject.GetComponent<WeaponSO>().modifier);
            TakeDamage(collision.collider.gameObject.GetComponent<WeaponSO>().Damage);

        }
        else if (collision.collider.tag == "Ability") {
            Apply(collision.collider.gameObject.GetComponent<AbilitySO>().modifier);
        }
    }

}
