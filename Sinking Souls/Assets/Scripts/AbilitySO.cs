using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Abilities")]
public class AbilitySO : ScriptableObject {

    public int cooldown;
    public float damage;
    private ModifierSO _modifier;
    public ModifierSO Modifier {
        get { return _modifier; }
        set { _modifier = value; }
    }

    public void Use(GameObject player) {

    }
}
