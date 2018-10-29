using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject {

    [TextArea] public string description;
    [HideInInspector] public bool elapsed = true;

	public abstract void Act(AIController controller);

}
