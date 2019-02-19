using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/IsMain")]
public class IsMain : Decision
{

    public override bool Decide(AIController controller)
    {
        return GameController.instance.mainEnemy == controller.gameObject;
    }

}