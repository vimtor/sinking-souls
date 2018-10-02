using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons")]
public class WeaponSO : ScriptableObject {

	public GameObject model;
    public float baseDamage;
    public float criticDamage;
    [HideInInspector]
    public float damage;
    public float useDelay;
    private ModifierSO _modifier;
    public ModifierSO Modifier {
        get { return _modifier;}
        set { _modifier = value; }
    }

    public void Instantiate(GameObject parent) {
        GameObject weapon = Instantiate(model, parent.transform);
        weapon.transform.parent = parent.transform;
    }
    public void Attack() {
        damage = baseDamage;
        model.GetComponent<BoxCollider>().enabled = true;
    }
    public void CriticAttack() {
        damage = criticDamage;
        model.GetComponent<BoxCollider>().enabled = true;
    }
}
