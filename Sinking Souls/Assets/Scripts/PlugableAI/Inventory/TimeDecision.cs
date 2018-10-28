using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/Time ")]
public class TimeDecision : Decision {

    public float min, max;

    private float selectedTime, elapsedTime = 0.0f;
    private bool timeSelected = false;

    public override bool Decide(AIController controller) {

        elapsedTime += Time.deltaTime;

        if (!timeSelected) {
            timeSelected = true;
            selectedTime = Random.Range(min, max);
            Debug.Log("Selected time: " + selectedTime);
        }

        if (elapsedTime > selectedTime) {
            timeSelected = false;
            elapsedTime = 0.0f;
            Debug.Log("Elapsed.");
            return true;
        }
        else {
            return false;
        }

    }

}
