﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Time")]
public class TimeDecision : Decision {

    [Tooltip("If this time has elapsed, it will return true.")]
    public float minimunTime;

    public override bool Decide(AIController controller)
    {
        return (controller.CheckIfTimeElapsed(minimunTime));
    }

}
