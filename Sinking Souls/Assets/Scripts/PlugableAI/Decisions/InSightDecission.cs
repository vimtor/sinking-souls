using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/InSight")]
public class InSightDecission : Decision {

    public float maxAngle;
    public float sightDist;

    public override bool Decide(AIController controller) {
        Vector3 targetDir = controller.player.transform.position - controller.gameObject.transform.position;
        float angle = Vector3.Angle(targetDir, controller.gameObject.transform.forward);
        Debug.Log(angle);
        Debug.Log(targetDir.magnitude);
        return ((angle < maxAngle/2) && targetDir.magnitude <= sightDist);

    }
}
