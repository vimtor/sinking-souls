using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/InSight")]
public class InSightDecission : Decision {

    public float maxAngle;
    public float sightDist;
    public float probavility = 1;

    public override bool Decide(AIController controller) {
        //if (Random.value <= probavility) return false;
        Vector3 targetDir = controller.player.transform.position - controller.gameObject.transform.position;
        float angle = Vector3.Angle(targetDir, controller.gameObject.transform.forward);
        if ((angle < maxAngle / 2) && targetDir.magnitude <= sightDist) return (Random.value <= probavility);
        return false;

    }
}
