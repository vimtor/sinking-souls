using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/WaterSplash")]
public class WaterSplashAbility : Ability {

    [Header("Configure Ability")]
    public float delay;
    public float range;
    public bool pickRandomPosition;
    public bool augmented;

    protected override void Configure(GameObject prefab) {
        if (!augmented) {
            prefab.GetComponent<WaterSplashBehaviour>().delay = delay;
            prefab.GetComponent<WaterSplashBehaviour>().range = range;
            prefab.GetComponent<WaterSplashBehaviour>().pickRandomPosition = pickRandomPosition;
        }
        else {
            prefab.GetComponent<WaterSplashSpawner>().delay = delay;
            prefab.GetComponent<WaterSplashSpawner>().range = range;
            prefab.GetComponent<WaterSplashSpawner>().pickRandomPosition = pickRandomPosition;
        }

    }

}
