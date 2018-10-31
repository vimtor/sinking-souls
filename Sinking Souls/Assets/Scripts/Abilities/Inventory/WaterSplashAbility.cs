using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/WaterSplash")]
public class WaterSplashAbility : Ability {

    [Tooltip("How much time after the casting has to pass to instantiate the water splash.")]
    [Range(0.0f, 3.0f)] public float delay;

    protected override void Configure(GameObject prefab) {
        prefab.GetComponent<WaterSplashBehaviour>().delay = delay;
    }

}
