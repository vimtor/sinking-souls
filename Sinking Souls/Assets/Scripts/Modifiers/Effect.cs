using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : ScriptableObject {

    public float strenght;
    public int duration;

    public abstract void Apply(GameObject other);
}
