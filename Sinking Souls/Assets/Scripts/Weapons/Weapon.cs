using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject {

    [Header("Weapon Information")]
	public GameObject model;
    new public string name;
    public string description;
    public Sprite sprite;
    
    [Header("Weapon Properties")]
    public float baseDamage;
    public float criticDamage;
    public Modifier modifier;

    [HideInInspector] public float damage;
    [HideInInspector] public GameObject weapon;

    private Vector3 originalSize;
    private BoxCollider boxCollider;
    public BoxCollider BoxCollider
    {
        get { return boxCollider; }
    }

    public void Instantiate(GameObject parent, GameObject owner)
    {
        weapon = Instantiate(model, parent.transform);
        weapon.transform.parent = parent.transform;

        weapon.AddComponent<WeaponHolder>().holder = this;
        weapon.GetComponent<WeaponHolder>().owner = owner;

        boxCollider = weapon.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            originalSize = boxCollider.size;
            boxCollider.enabled = false;
        }

        damage = baseDamage;
    }

    public void Attack() {
        damage = baseDamage;
    }

    public void CriticAttack() {
        Debug.Log("Critical");
        damage = criticDamage;
    }

    public void GrowCollision(int mult) {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize * mult;
    }

    public void ShrinkCollision() {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize;
    }

    public void EnableCollider()
    {
        boxCollider.enabled = true;
    }

    public void DisableCollider()
    {
        boxCollider.enabled = false;
    }

}
