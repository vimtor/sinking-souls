using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/WaterSplash")]
public class WaterSplashAbility : Ability {

    [Header("Configure Ability")]
    public float delay;
    public float range;
    public bool pickRandomPosition;

    protected override void Configure(GameObject prefab) {
        prefab.GetComponent<WaterSplashBehaviour>().delay = delay;
        prefab.GetComponent<WaterSplashBehaviour>().range = range;
        prefab.GetComponent<WaterSplashBehaviour>().pickRandomPosition = pickRandomPosition;
    }

}
