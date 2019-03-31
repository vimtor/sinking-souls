using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialoguePlayableBehaviour : PlayableBehaviour
{
    public Dialogue dialogue;
    public TimelineAsset timeline;

    private bool canPlayNext;
    private bool firstTime = true;

    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (firstTime)
        {
            firstTime = false;
            DialogueManager.Instance.DisplayDialogue(dialogue);
        }

        if (Input.GetKey(KeyCode.K))
        {
            canPlayNext = false;
            DialogueManager.Instance.HideDialogue();
            CinematicManager.Instance.Play(timeline);
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

        if (canPlayNext) NextCinematic();
    }

    private void NextCinematic()
    {
        DialogueManager.Instance.HideDialogue();
        CinematicManager.Instance.Play(timeline);
    }
}
