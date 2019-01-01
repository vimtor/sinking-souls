using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/MagicCanon")]
public class AilinMagicCanon : Action
{

    [Range(0.0f, 1.0f)] public float actionFrame;

    public Ability spell;

    private float clipLength;

    public override void Act(AIController controller)
    {

        controller.SetAnimBool("SPELL");

        if (!controller.GetComponent<Enemy>().AbilityThrown && (controller.CheckIfCountDownElapsed(clipLength * actionFrame)))
        {
            controller.gameObject.GetComponent<Enemy>().ability = spell;
            controller.GetComponent<Enemy>().ability.Use(controller.gameObject);
        }
    }

}