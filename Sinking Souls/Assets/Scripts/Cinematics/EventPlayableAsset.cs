using UnityEngine;
using UnityEngine.Playables;

public class EventPlayableAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<EventPlayableBehaviour>.Create(graph);
    }
}
