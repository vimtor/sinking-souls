using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/AilinTeleport")]
public class AilinTeleportAction : Action
{

    public List<GameObject> teleportPoints;
    private Vector3 newPosition;

    public override void Act(AIController controller)
    {
        int maxDistance = 0;
        foreach(GameObject tp in teleportPoints)
        {
            if(Vector3.Distance(tp.transform.position, controller.player.transform.position) > maxDistance) newPosition = tp.transform.position;
        }
        controller.transform.position = newPosition;
    }
}
