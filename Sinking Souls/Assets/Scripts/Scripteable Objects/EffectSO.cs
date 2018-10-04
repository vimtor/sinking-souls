using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/")]
public abstract class EffectSO : ScriptableObject {

    public float strenght;
    public int duration;

    public void Apply(GameObject other){}
}
