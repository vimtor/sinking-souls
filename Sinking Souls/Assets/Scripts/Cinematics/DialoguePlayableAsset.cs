using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialoguePlayableAsset : PlayableAsset
{
    public Dialogue dialogue;
    public TimelineAsset timeline;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialoguePlayableBehaviour>.Create(graph);

        var dialogueBehaviour = playable.GetBehaviour();
        dialogueBehaviour.dialogue = dialogue;
        dialogueBehaviour.timeline = timeline;

        return playable;
    }
}
