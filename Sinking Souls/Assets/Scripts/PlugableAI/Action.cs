using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject {

    [HideInInspector] public bool elapsed = true;

    public virtual void Act(AIController controller) { }
    public virtual void Act(AilinBoss controller) { }

}
