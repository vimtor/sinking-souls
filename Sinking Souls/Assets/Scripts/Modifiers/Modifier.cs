using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifier")]
public class Modifier : ScriptableObject {

    public Sprite sprite;
    new public string name;
    public string description;
    public List<Effect> effects;
    public int price;
    public bool owned;
    public bool unlocked;


    void Buy() {

    }


}