using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Bomb")]
public class BombAbility : Ability
{
    [Header("Specific Properties")]
    public float explotionForce;
    public GameObject effect;

    protected override void Configure(GameObject bomb)
    {
        bomb.GetComponent<Rigidbody>().AddForce(parent.transform.forward * 50);
        bomb.GetComponent<BombBehaviour>().explotionForce = explotionForce;
        bomb.GetComponent<BombBehaviour>().effect = effect;
    }

}
