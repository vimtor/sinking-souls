using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Arrow")]
public class ArrowAbility : Ability {

    [Tooltip("Speed with which the arrow will travel through air.")]
    public float arrowForce;

    public override void Use(GameObject parent) {

        entity = parent.GetComponent<Entity>();

        if (!entity.thrown) {

            GameObject arrow = SetPrefab(parent);
            arrow.transform.localRotation = parent.transform.localRotation;
            arrow.GetComponent<ArrowBehaviour>().direction = parent.transform.forward;
            arrow.GetComponent<ArrowBehaviour>().arrowForce = arrowForce;

            entity.thrown = true;

        }

    }
}
