using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class WeaponSO : ScriptableObject {

	public GameObject model;

    public float baseDamage;
    public float criticDamage;
    private float _damage;
    public float Damage {
        set { throw new System.Exception("You cannot set the damage of a weapon."); }
        get { return _damage; }
    }

    public float useDelay;
    public ModifierSO modifier;
    
    private BoxCollider collider;

    private void Awake() {
        collider = model.GetComponent<BoxCollider>();
    }

    public void Instantiate(GameObject parent) {
        GameObject weapon = Instantiate(model, parent.transform);
        weapon.transform.parent = parent.transform;
    }

    public void Attack() {
        _damage = baseDamage;
        collider.enabled = true;
    }

    public void CriticAttack() {
        _damage = criticDamage;
        collider.enabled = true;
    }
}
