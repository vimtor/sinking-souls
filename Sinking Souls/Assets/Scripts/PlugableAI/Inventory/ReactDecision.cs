using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/React")]
public class ReactDecision : Decision {

    public override bool Decide(AIController controller) {

        return controller.GetComponent<Enemy>().hit;

    }
}
