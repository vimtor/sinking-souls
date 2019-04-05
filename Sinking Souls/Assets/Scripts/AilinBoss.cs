using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AilinBoss : AIController {

    [HideInInspector] public float timeInRange;
    [HideInInspector] public float rageTime;
    [HideInInspector] public bool rageMode;
    [HideInInspector] public bool firstAttack;
    [HideInInspector] public bool stateFinished;


    public float TimeToRage;


    public void Start()
    {   
        timeInRange = 0;
        rageTime = 0;
        rageMode = false;
        firstAttack = false;
        stateFinished = true;
    }

	
	// Update is called once per frame
	protected void Update () {
        timeInRange += Time.deltaTime;
        rageTime += Time.deltaTime;
        if (rageTime >= TimeToRage) rageMode = true;
    }

    public bool CheckIfTimeTranscurred(float duration)
    {
        return (timeInRange >= duration);
    }
}
