using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always returns true.
/// </summary>
[CreateAssetMenu(menuName = "PlugableAI/Decisions/Empty")]
public class EmptyDecision : Decision {

    public override bool Decide(AIController controller) {
        return true;
    }

}
