using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public int cooldown;
    public float damage;
    public Modifier modifier;
    public GameObject prefab;

    [HideInInspector] public string target;
    [HideInInspector] public Entity entity;

    public abstract void Use(GameObject parent);

    public GameObject SetPrefab(GameObject parent) {
        GameObject instantiated = Instantiate(prefab);
        instantiated.transform.position = entity.hand.transform.position;

        target = parent.gameObject.tag == "Player" ? "Enemy" : "Player";
        instantiated.AddComponent<AbilityHolder>().holder = this;

        return instantiated;
    }

}
