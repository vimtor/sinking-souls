using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/MultipleArrow")]
public class MultipleArrowAbility : Ability {

    [Tooltip("Speed with which the arrow will travel through air.")]
    public float arrowForce;

    protected override void Configure(GameObject augmentedArrow)
    {
        for (int i = 0; i < 3; i++)
        {
            augmentedArrow.transform.GetChild(i).gameObject.AddComponent<AbilityHolder>().holder = augmentedArrow.GetComponent<AbilityHolder>().holder;
            augmentedArrow.transform.GetChild(i).transform.localRotation = parent.transform.localRotation;
            augmentedArrow.transform.GetChild(i).GetComponent<ArrowBehaviour>().arrowForce = arrowForce;

        }

        augmentedArrow.transform.GetChild(1).GetComponent<ArrowBehaviour>().direction = Vector3.Normalize(parent.transform.forward * 10 + (parent.transform.right * -1));
        augmentedArrow.transform.GetChild(0).GetComponent<ArrowBehaviour>().direction = parent.transform.forward;
        augmentedArrow.transform.GetChild(2).GetComponent<ArrowBehaviour>().direction = Vector3.Normalize(parent.transform.forward * 10 + (parent.transform.right));
    }
}
