using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Decisions/AilinAttackSelector")]
public class AilinAttackSelectorDecision : Decision
{
    [Header("States")]
    [Header("Normal")]
    public State magicCanonState;
    public State expansiveWaveState;  
    public State waterPilarsState;
    public State tp;
    [Header("RageMode")]
    public State magicCanonStateRage;
    public State expansiveWaveStateRage;
    public State waterPilarsStateRage;

    [Header("Configuration")]
    public float maxAngle;
    public int expansionWaveRange;

    public override bool Decide(AIController controller)
    {
    int rand = Random.Range(0, 100);
        //Normal mode
        #region NormalMode
        if (!controller.gameObject.GetComponent<AilinAIAsistant>().rageMode)
        {
            //If player is not in expansion wave range
            if (Vector3.Distance(controller.player.transform.position, controller.transform.position) > expansionWaveRange)
            {
                //If player is going straight to the enemy
                if (PlayerTrajectory(controller))
                {
                    if (rand <= 65) controller.CurrentState.transitions[0].trueState = magicCanonState;             // 65%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsState;                       // 35%
                }
                else
                {
                    if (rand <= 35) controller.CurrentState.transitions[0].trueState = magicCanonState;             // 35%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsState;                       // 65%
                }
            }
            //If playes is in expansion wave range
            else
            {
                //If player is going straight to the enemy
                if (PlayerTrajectory(controller))
                {
                    if (rand <= 50) controller.CurrentState.transitions[0].trueState = magicCanonState;             // 50%
                    else if (rand <= 80) controller.CurrentState.transitions[0].trueState = expansiveWaveState;     // 30%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsState;                       // 20%
                }
                else
                {
                    if (rand <= 20) controller.CurrentState.transitions[0].trueState = magicCanonState;             // 20%
                    else if (rand <= 64) controller.CurrentState.transitions[0].trueState = expansiveWaveState;     // 45%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsState;                       // 35%
                }
            }
        }
        #endregion

        //Rage mode
        #region RageMode
        else
        {
            //Start rage mode
            if (!controller.gameObject.GetComponent<AilinAIAsistant>().firstAttack)
            {
                controller.gameObject.GetComponent<AilinAIAsistant>().firstAttack = true;
                controller.CurrentState.transitions[0].trueState = expansiveWaveStateRage;
            }
            else
            {
                //If player is going straight to the enemy
                if (PlayerTrajectory(controller))
                {
                    if (rand <= 20) controller.CurrentState.transitions[0].trueState = magicCanonStateRage;             // 20%
                    else if (rand <= 70) controller.CurrentState.transitions[0].trueState = expansiveWaveStateRage;     // 50%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsStateRage;                       // 30%
                }
                else
                {
                    if (rand <= 20) controller.CurrentState.transitions[0].trueState = magicCanonStateRage;             // 20%
                    else if (rand <= 60) controller.CurrentState.transitions[0].trueState = expansiveWaveStateRage;     // 40%
                    else controller.CurrentState.transitions[0].trueState = waterPilarsStateRage;                       // 40%
                }
            }
        }
        #endregion
        if(controller.gameObject.GetComponent<AilinAIAsistant>().tpCounter > controller.gameObject.GetComponent<AilinAIAsistant>().tpTime) {
            controller.CurrentState.transitions[0].trueState = tp;
            controller.gameObject.GetComponent<AilinAIAsistant>().tpCounter = 0;
        }
        return true;
    }

    //Checks if player is going  straight to the enemy
    public bool PlayerTrajectory(AIController controller)
    {
        Vector3 targetDir = controller.gameObject.transform.position - controller.player.transform.position;
        float angle = Vector3.Angle(targetDir, controller.player.transform.forward);

        if (angle < maxAngle / 2) return true;
  
        return false;
    }
}