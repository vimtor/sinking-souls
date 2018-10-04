using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability")]
public class AbilitySO : ScriptableObject {

    public int cooldown;
    public float damage;
    public ModifierSO modifier;

    public virtual void Use(GameObject player) {

    }
}
