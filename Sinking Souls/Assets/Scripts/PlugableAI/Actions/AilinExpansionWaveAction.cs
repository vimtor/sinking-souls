using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/Actions/AilinExpansionWave")]
public class AilinExpansionWaveAction : Action
{
    public float delayTime;
    public bool waitAnimation;

    public override void Act(AilinBoss controller)
    {
        controller.stateFinished = false;
        if (!controller.rageMode) GameController.instance.StartCoroutine(delay(delayTime, 1, controller));
        else
        {
            GameController.instance.StartCoroutine(delay(delayTime, 3, controller));
        }
    }

    /// <summary>
    /// Specify the delay, number of iterations and AIController
    /// </summary>
    /// <param name="time"></param>
    /// <param name="times"></param>
    /// <param name="controller"></param>
    /// <returns></returns>
    IEnumerator delay(float time, int times, AilinBoss controller)
    {
        // controller.GetComponent<Enemy>().ability.Use(controller.gameObject, controller.transform);
        yield return new WaitForSeconds(time);
        if (times - 1 > 0) GameController.instance.StartCoroutine(delay(delayTime, times - 1, controller));
        else controller.stateFinished = true;
    }

}
