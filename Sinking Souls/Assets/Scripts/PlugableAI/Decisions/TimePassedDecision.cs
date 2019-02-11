using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/TimePassedDecision")]

public class TimePassedDecision : Decision
{
    public float duration;
    public override bool Decide(AIController controller)
    {
        return controller.CheckIfCountDownElapsed(duration);
    }
}
