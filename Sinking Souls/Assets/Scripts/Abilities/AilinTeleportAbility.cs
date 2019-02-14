using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AilinTeleport")]
public class AilinTeleportAbility : Ability {

    private GameObject point;
    private GameObject lastPoint = null;
    private float maxDist = 0;

    public override void Activate() {
        maxDist = 0;

        foreach(GameObject sp in GameController.instance.LevelGenerator.lastRoom.transform.parent.GetComponent<SpawnController>().spawnPoints) {
            if(Vector3.Distance(GameController.instance.player.transform.position, sp.transform.position) > maxDist && sp !=lastPoint) {
                maxDist = Vector3.Distance(parent.transform.position, sp.transform.position);
                point = sp;

            }
        }

        parent.transform.position = point.transform.position;
        lastPoint = point;
    }
}
