using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/SpecialArrow")]
public class SpecialArrow : Action
{

    [Range(0.0f, 1.0f)] public float actionFrame;

    public Ability arrow;

    private float clipLength;

    public override void Act(AIController controller)
    {

        clipLength = controller.GetComponent<Enemy>().clipLength["SpellAnim"];
        controller.SetAnimBool("SPELL");

        if (!controller.GetComponent<Enemy>().thrown && (controller.CheckIfCountDownElapsed(clipLength * actionFrame)))
        {
            controller.gameObject.GetComponent<Enemy>().ability = arrow;
            controller.GetComponent<Enemy>().ability.Use(controller.gameObject);
        }


        if (controller.CheckIfCountDownElapsed(controller.GetComponent<Enemy>().clipLength["SpellAnim"]))
        {
            controller.stateTimeElapsed = 0;
            controller.GetComponent<Enemy>().thrown = false;

        }

    }

}