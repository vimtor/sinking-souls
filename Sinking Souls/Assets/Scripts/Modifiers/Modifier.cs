using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modifier")]
public class Modifier : ScriptableObject {

    public Sprite sprite;
    new public string name;
    public string description;
    public List<Effect> effects;

    [HideInInspector] public bool owned;
    [HideInInspector] public bool unlocked;

    public Dictionary<Enemy.EnemyType, int> prices;

    void Buy() {

    }


}