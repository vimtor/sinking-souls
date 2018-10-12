using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlugableAI/State")]
public class State : ScriptableObject {

    public Action[] actions;
    public Transition[] transitions;

    public void UpdateState(AIController controller) {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(AIController controller) {
        foreach (Action action in actions) {
            action.Act(controller);
        }
    }

    private void CheckTransitions(AIController controller) {
        foreach (Transition transition in transitions) {
            if (transition.decision.Decide(controller)) {
                controller.TransitionToState(transition.trueState);
            }
            else {
                controller.TransitionToState(transition.falseState);
            }
        }
    }

}
