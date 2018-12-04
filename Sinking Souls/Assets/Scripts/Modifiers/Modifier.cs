using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : ScriptableObject {

    public Sprite sprite;
    new public string name;
    public string description;
    public int price;
    public bool owned;
    public bool unlocked;

    public int damage;
    public float hitTime;
    public float duration;


    void Buy() {}
    public abstract void Apply(GameObject go);


}