using UnityEngine.Playables;

public class EndGamePlayableBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        ApplicationManager.Instance.FinishGame();
    }
}
