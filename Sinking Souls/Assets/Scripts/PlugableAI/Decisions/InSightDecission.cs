using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/InSight")]
public class InSightDecission : Decision {

    public bool lessThan = true;

    public float maxAngle;
    public float sightDist;
    public float probavility = 1;

    public override bool Decide(AIController controller) {
        //if (Random.value <= probavility) return false;
        Vector3 targetDir = controller.player.transform.position - controller.gameObject.transform.position;
        float angle = Vector3.Angle(targetDir, controller.gameObject.transform.forward);
        Debug.Log(targetDir.magnitude);
        if (lessThan) { if ((angle < maxAngle / 2) && targetDir.magnitude <= sightDist) return (Random.value <= probavility); }
        else
        {
            Debug.Log("InRange");
            if ((angle < maxAngle / 2) && targetDir.magnitude >= sightDist) return (Random.value <= probavility);
        }
       
        return false;
    }
}
