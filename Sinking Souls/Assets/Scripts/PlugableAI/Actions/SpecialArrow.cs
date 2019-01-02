using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/SpecialArrow")]
public class SpecialArrow : Action
{
    public Ability arrow;

    public override void UpdateAction(AIController controller)
    {
        controller.SetAnimBool("SPELL");
    }

}