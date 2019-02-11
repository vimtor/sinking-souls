using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/ProvabilityBasedDistanceDecision")]

public class ProvabilityBasedDistanceDecision : Decision
{
    public bool directlyProportional = true;
    public Vector2 originalRange;
    public override bool Decide(AIController controller)
    {
        RaycastHit hit;
        float distance = (controller.transform.position - controller.player.transform.position).magnitude;
        Physics.Raycast(controller.transform.position, (controller.player.transform.position - controller.transform.position), out hit, Mathf.Infinity);
        if(hit.transform.tag == "Player")
        {
            if (directlyProportional)
            {
                return Random.Range(0, 100) <= controller.player.GetComponent<Player>().map(distance, originalRange.x, originalRange.y, 0, 100);
            }
            else
                return Random.Range(0, 100) <= controller.player.GetComponent<Player>().map(distance, originalRange.x, originalRange.y, 100, 0);
        }
        return false;
    }
}
