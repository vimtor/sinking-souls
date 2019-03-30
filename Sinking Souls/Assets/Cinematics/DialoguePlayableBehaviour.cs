using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialoguePlayableBehaviour : PlayableBehaviour
{
    public Dialogue dialogue;
    public PlayableDirector director;
    public TimelineAsset timeline;

    private bool canPlayNext;

    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Debug.Log(dialogue.name);

        if (Input.GetKey(KeyCode.K))
        {
            director.Play(timeline);
        }
    }

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        canPlayNext = true;
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
        if (canPlayNext) director.Play(timeline);
    }
}
