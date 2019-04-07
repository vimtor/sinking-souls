using UnityEngine.Playables;

public class EndGamePlayableBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        SaveManager.Reset();
        ApplicationManager.Instance.ChangeScene(ApplicationManager.GameState.MAIN_MENU);
    }
}
