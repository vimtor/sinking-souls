using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject {

    [Header("Weapon Information")]
	public GameObject model;
    new public string name;
    public string description;
    public Sprite sprite;
    
    [Header("Weapon Properties")]
    public float useDelay;
    public float baseDamage;
    public float criticDamage;
    public Modifier modifier;

    [HideInInspector] public float damage;
    [HideInInspector] public bool hitting;
    [HideInInspector] public GameObject weapon;

    private Vector3 originalSize;
    private BoxCollider boxCollider;

    private void Start() {
        boxCollider = model.GetComponent<BoxCollider>();
    }

    public void Instantiate(GameObject parent, GameObject owner) {
        weapon = Instantiate(model, parent.transform);
        weapon.transform.parent = parent.transform;
        weapon.AddComponent<WeaponHolder>().holder = this;
        weapon.GetComponent<WeaponHolder>().owner = owner;
        if (weapon.GetComponent<BoxCollider>()) originalSize = weapon.GetComponent<BoxCollider>().size;
    }

    public void Attack() {
        damage = baseDamage;
        hitting = true;
    }

    public void CriticAttack() {
        damage = criticDamage;
        hitting = true;
    }

    public void GrowCollision(int mult) {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize * mult;
    }

    public void ShrinkCollision() {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize;
    }

}
