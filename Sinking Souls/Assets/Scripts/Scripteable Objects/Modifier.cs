using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modifier : ScriptableObject {

    public Sprite sprite;
    new public string name;
    public string description;
    public List<Effect> effects;

    private bool owned;
    public Dictionary<Enemy.EnemyType, int> prices;

    void Buy() {

    }

    void CanBuy() {

    }

}