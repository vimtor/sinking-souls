using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public int cooldown;
    public float Damage;
    public Modifier modifier;
    public GameObject prefab;
    public string target;

    public abstract void Use(GameObject player);
}
