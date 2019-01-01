using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/SpecialArrow")]
public class SpecialArrow : Action
{
    public Ability arrow;

    public override void Act(AIController controller)
    {
        controller.SetAnimBool("SPELL");
    }

}