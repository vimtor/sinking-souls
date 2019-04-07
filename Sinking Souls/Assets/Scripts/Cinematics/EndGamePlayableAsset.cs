using UnityEngine;
using UnityEngine.Playables;

public class EndGamePlayableAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<EndGamePlayableBehaviour>.Create(graph);
    }
}
