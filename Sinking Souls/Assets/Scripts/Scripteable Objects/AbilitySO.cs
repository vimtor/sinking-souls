using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability")]
public class AbilitySO : ScriptableObject {

    public int cooldown;
    public float Damage;
    public ModifierSO modifier;
    public GameObject prefab;
    public string targget;

    public virtual void Use(GameObject player) {

    }
}
