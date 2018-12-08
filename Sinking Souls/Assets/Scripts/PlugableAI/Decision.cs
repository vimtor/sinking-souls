using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Decision : ScriptableObject {

    public virtual bool Decide(AIController controller) { return false; }
    public virtual bool Decide(AilinBoss controller) { return false; }

}
