using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlugableAI/Decisions/ObstacleDecision")]

public class ObstacleDecision : Decision
{
    [Tooltip("percentage (0 to 100)")]
    public int probability;
    public float minDistance;
    public bool closer = false;
    private int layerMask = ~((1 << 12) | (1 << 2) | (1 << 16));

    public override bool Decide(AIController controller)
    {
        RaycastHit hit;
        float distance = (controller.transform.position - controller.player.transform.position).magnitude;
        Physics.Raycast(controller.transform.position, (controller.player.transform.position - controller.transform.position), out hit, Mathf.Infinity, layerMask);
        int result = Random.Range(0, 100);
        Debug.Log(hit.transform.name);

        if (closer) return (distance <= minDistance && hit.transform.tag == "Player" && result <= probability);
        else
        return (distance >= minDistance && hit.transform.tag == "Player" && result <= probability);
    }

}
