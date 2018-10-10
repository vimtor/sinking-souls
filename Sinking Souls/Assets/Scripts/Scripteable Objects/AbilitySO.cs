using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability")]
public class AbilitySO : ScriptableObject {

    public int cooldown;
    public float Damage;
    public Modifier modifier;
    public GameObject prefab;
    public string target;

    public virtual void Use(GameObject player) {

    }
}
