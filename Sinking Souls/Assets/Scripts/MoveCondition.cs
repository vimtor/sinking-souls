using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Message Conditions/ Move Condition")]
public class MoveCondition : MessageCondition {
    [HideInInspector] public float time = 0;
    public float extraDelay;
    public float waitTime;

    public override bool Check() {
        if(InputManager.LeftJoystick.magnitude != 0) {
            time = 0;
        }
        else {
            if (completed) {
                if (time >= waitTime + extraDelay) {
                    //completed = true;
                    return true;
                }

            }
            else {
                if (time >= waitTime) {
                    //completed = true;
                    return true;
                }
            }

            time += Time.deltaTime;
        }
        return false;
    }

    public override void reStart(bool deComplete) {
        time = 0;
        if(deComplete)completed = false;
    }
}
