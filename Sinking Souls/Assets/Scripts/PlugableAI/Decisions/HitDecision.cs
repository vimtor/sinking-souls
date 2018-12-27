using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Hit")]
public class HitDecision : Decision {

    public override bool Decide(AIController controller) {

        return controller.GetComponent<Enemy>().Hitted;

    }
}
