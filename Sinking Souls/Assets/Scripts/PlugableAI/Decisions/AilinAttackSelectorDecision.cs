using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/AilinAttackSelector")]
public class AilinAttackSelectorDecision : Decision
{
    [Header("States")]
    public State magicCanonState;
    public State expansiveWaveState;  
    public State waterPilarsState;

    [Header("Configuration")]
    public float maxAngle;
    public int expansionWaveRange;

    public override bool Decide(AilinBoss controller)
    {
        int rand = Random.Range(0, 100);
        if (controller.stateFinished)
        {
            //Normal mode
            #region NormalMode
            if (!controller.rageMode)
            {
                //If player is not in expansion wave range
                if (Vector3.Distance(controller.player.transform.position, controller.transform.position) > expansionWaveRange)
                {
                    //If player is going straight to the enemy
                    if (PlayerTrajectory(controller))
                    {
                        if (rand <= 65) controller.currentState.transitions[0].trueState = magicCanonState;             // 65%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 35%
                    }
                    else
                    {
                        if (rand <= 35) controller.currentState.transitions[0].trueState = magicCanonState;             // 35%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 65%
                    }
                }
                //If playes is in expansion wave range
                else
                {
                    //If player is going straight to the enemy
                    if (PlayerTrajectory(controller))
                    {
                        if (rand <= 50) controller.currentState.transitions[0].trueState = magicCanonState;             // 50%
                        else if (rand <= 80) controller.currentState.transitions[0].trueState = expansiveWaveState;     // 30%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 20%
                    }
                    else
                    {
                        if (rand <= 20) controller.currentState.transitions[0].trueState = magicCanonState;             // 20%
                        else if (rand <= 64) controller.currentState.transitions[0].trueState = expansiveWaveState;     // 45%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 35%
                    }
                }
            }
            #endregion

            //Rage mode
            #region RageMode
            else
            {
                //Start rage mode
                if (!controller.firstAttack)
                {
                    controller.firstAttack = true;
                    controller.currentState.transitions[0].trueState = expansiveWaveState;
                }
                else
                {
                    //If player is going straight to the enemy
                    if (PlayerTrajectory(controller))
                    {
                        if (rand <= 20) controller.currentState.transitions[0].trueState = magicCanonState;             // 20%
                        else if (rand <= 70) controller.currentState.transitions[0].trueState = expansiveWaveState;     // 50%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 30%
                    }
                    else
                    {
                        if (rand <= 20) controller.currentState.transitions[0].trueState = magicCanonState;             // 20%
                        else if (rand <= 60) controller.currentState.transitions[0].trueState = expansiveWaveState;     // 40%
                        else controller.currentState.transitions[0].trueState = waterPilarsState;                       // 40%
                    }
                }
            }
            #endregion

            return true;
        }

        return false;
    }

    //Checks if player is going  straight to the enemy
    public bool PlayerTrajectory(AilinBoss controller)
    {
        Vector3 targetDir = controller.gameObject.transform.position - controller.player.transform.position;
        float angle = Vector3.Angle(targetDir, controller.player.transform.forward);

        if (angle < maxAngle / 2) return true;
  
        return false;
    }
}