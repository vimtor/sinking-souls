using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier : ScriptableObject {

    [Header("Ability Information")]
    new public string name;
    public string description;
    public int price;
    public Sprite sprite;
    public int tier;

    [Header("General Properties")]
    public bool owned;
    public bool picked;

    [Space(10)]
    public int damage;
    public float hitTime;
    public float duration;
    

    


    void Buy() {}
    public abstract void Apply(GameObject go);


}