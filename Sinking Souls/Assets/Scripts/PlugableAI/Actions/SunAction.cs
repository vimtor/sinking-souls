using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Stun")]
public class SunAction : Action {

    public override void UpdateAction(AIController controller) {
        controller.SetAnimBool("IDLE");///CHANGE THIS TO STUN WHEN HAVING THE ANIMATION
    }

}
