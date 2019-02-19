using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/IsCasual")]
public class IsCasual : Decision
{

    public override bool Decide(AIController controller)
    {
        if (GameController.instance.casualEnemy == controller.gameObject)
        {
            GameController.instance.casualEnemy = null;
            return true;
        }
        return false;
    }
}