using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons")]
public class WeaponSO : ScriptableObject {

	public GameObject model;
    public float damage;
    public float dps;
    private ModifierSO _modifier;
    public ModifierSO Modifier {
        get { return _modifier;}
        set { _modifier = value; }
    }
}
