using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class AIController : MonoBehaviour {

    public State currentState;
    public State remainState;
    public GameObject player;
    public Animator enemyAnimator;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public float stateTimeElapsed;


    private bool aiActive;

    public void SetupAI() {
        stateTimeElapsed = 0;
        aiActive = true;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Enemy>().animator;

        if (aiActive) navMeshAgent.enabled = true;
    }

    void Update() {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
        Debug.Log(currentState.name);
    }

    public void TransitionToState(State nextState) {
        if(nextState != remainState) {
            currentState = nextState;
            OnExitState();
        }

        enemyAnimator.SetBool("RUN", false);
        enemyAnimator.SetBool("IDLE", false);
    }

    public bool CheckIfCountDownElapsed(float duration) {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState() {
        stateTimeElapsed = 0;
    }

    public void SetAnimBool(string str) {

        enemyAnimator.SetBool("RUN", false);
        enemyAnimator.SetBool("IDLE", false);

        enemyAnimator.SetBool(str, true);
    }

}
