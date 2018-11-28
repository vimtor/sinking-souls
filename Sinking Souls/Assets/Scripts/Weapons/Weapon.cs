using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject {

	public GameObject model;
    public GameObject weapon;
    private Vector3 originalSize;

    public float baseDamage;
    public float criticDamage;
    public bool hitting;

    private float _damage;
    public float Damage {
        set { throw new System.Exception("You cannot set the damage of a weapon."); }
        get { return _damage; }
    }

    public float useDelay;
    public Modifier modifier;
    
    private BoxCollider boxCollider;

    private void Start() {
        boxCollider = model.GetComponent<BoxCollider>();
    }

    public void Instantiate(GameObject parent, GameObject owner) {
        weapon = Instantiate(model, parent.transform);
        weapon.transform.parent = parent.transform;
        weapon.AddComponent<WeaponHolder>().holder = this;
        weapon.GetComponent<WeaponHolder>().owner = owner;
        if(weapon.GetComponent<BoxCollider>()) originalSize = weapon.GetComponent<BoxCollider>().size;
    }

    public void Attack() {
        _damage = baseDamage;
        hitting = true;
    }

    public void CriticAttack() {
        _damage = criticDamage;
        hitting = true;
    }

    public void GrowCollision(int mult) {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize * mult;
    }

    public void ShrinkCollision() {
        if (weapon.GetComponent<BoxCollider>()) weapon.GetComponent<BoxCollider>().size = originalSize;
    }

}
