using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {

    private float life;
    private WeaponSO weapon;

    public float walkSpeed;
    public ModifierSO baseModifier;

    public void Apply(GameObject other) {
        List<EffectSO> effects = other.GetComponent<WeaponSO>().Modifier.effects;
        foreach(EffectSO effect in effects) {
            effect.Apply(this.gameObject);
        }
    }
}
