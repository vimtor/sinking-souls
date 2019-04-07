using UnityEngine;
using UnityEngine.Playables;

public class TransitionFixBehaviour : PlayableBehaviour
{
    private GameObject cutsceneWrapper;
    private int frameCount;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        cutsceneWrapper = GameObject.FindGameObjectWithTag("CutsceneWrapper");
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (frameCount == 0)
        {
            cutsceneWrapper.SetActive(false);
            frameCount++;
        }
        else if (frameCount == 1)
        {
            cutsceneWrapper.SetActive(true);
            frameCount++;
        }
    }
}
