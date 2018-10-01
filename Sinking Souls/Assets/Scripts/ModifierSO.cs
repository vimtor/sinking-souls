using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "Modifiers")]
public class ModifierSO : ScriptableObject
{

    public Sprite sprite;
    public string name;
    public string description;
    public List<EffectSO> effects;

    private bool owned;
    public Dictionary<Enemy.EnemyType, int> prices;

    void Buy()
    {

    }
    void CanBuy()
    {

    }

}