using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    [HideInInspector] public bool elapsed = true;
    // Is only executed once until the state checks transitions.
    public virtual void StartAction(AIController controller) { }

    // Executed every frame.
    public virtual void UpdateAction(AIController controller) { }

    public virtual void Act(AilinBoss controller) { }
}
