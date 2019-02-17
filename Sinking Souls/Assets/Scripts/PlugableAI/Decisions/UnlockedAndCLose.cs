using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Decisions/UnlockedAndCLose")]

public class UnlockedAndCLose : Decision {

    public float distance;

    public override bool Decide(AIController controller)
    {
        return (Vector3.Distance(controller.player.transform.position, controller.transform.position) <= distance && controller.player.GetComponent<Player>().lockedEnemy != controller.gameObject);
    }
}
