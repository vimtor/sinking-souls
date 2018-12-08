using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AilinBoss : AIController {

    [HideInInspector] public float timeInRange;
    [HideInInspector] public float rageTime;
    [HideInInspector] public bool rageMode;
    public float TimeToRage;


    public override void SetupAI()
    {
        base.SetupAI();
        timeInRange = 0;
        rageTime = 0;
        rageMode = false;
    }

	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        timeInRange += Time.deltaTime;
        rageTime += Time.deltaTime;
        if (rageTime >= TimeToRage) rageMode = true;
    }

    public bool CheckIfTimeTranscurred(float duration)
    {
        return (timeInRange >= duration);
    }
}
