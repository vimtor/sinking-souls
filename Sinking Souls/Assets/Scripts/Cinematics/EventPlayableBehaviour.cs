using UnityEngine;
using UnityEngine.Playables;
using System;

public class EventPlayableBehaviour : PlayableBehaviour
{
    private bool firstFrame;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (firstFrame) return;

        firstFrame = true;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var klauss = Array.Find(enemies, enemy => enemy.name.Contains("Klaus"));

        klauss.GetComponent<KlausBossAI>().SetupAI();
    }
}
