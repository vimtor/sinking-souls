using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/Heal")]
public class HeallAllEnemies : Action {
    public float healInterval;
    public float healAmount;
    private float time = 0;

    public override void StartAction(AIController controller) {
        //time = 0;
        base.StartAction(controller);
        elapsed = true;
        Debug.Log("Started");
    }

    public override void UpdateAction(AIController controller) {

        if (time >= healInterval) {
            Debug.Log("Healed");
            heal();
            time = 0;
        }

        time += Time.deltaTime;
    }

    private void heal() {
        foreach(GameObject en in GameController.instance.roomEnemies) en.GetComponent<Entity>().Heal(healAmount);
    }
}
