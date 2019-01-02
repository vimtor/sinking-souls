using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/State")]
public class State : ScriptableObject {

    public Action[] actions;
    public Transition[] transitions;

    private bool m_ElapsedActions = true;
    private bool m_InitialActionsDone = false;


    public void UpdateState(AIController controller)
    { 
        if (!m_InitialActionsDone) StartActions(controller);
        UpdateActions(controller);

        if (m_ElapsedActions) CheckTransitions(controller);
    }

    private void StartActions(AIController controller)
    {
        foreach (var action in actions)
        {
            action.StartAction(controller);
        }

        m_InitialActionsDone = true;
    }

    private void UpdateActions(AIController controller)
    {
        m_ElapsedActions = true;
        foreach (Action action in actions)
        {
            action.UpdateAction(controller);
            m_ElapsedActions = m_ElapsedActions && action.elapsed;
        }
    }

    private void CheckTransitions(AIController controller)
    {
        foreach (Transition transition in transitions)
        {
            if (transition.decision.Decide(controller))
            {
                controller.TransitionToState(transition.trueState);
            }
            else {
                controller.TransitionToState(transition.falseState);
            }
        }

        controller.stateTimeElapsed = 0.0f;
        m_InitialActionsDone = false;
    }

}
