using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CinematicTrigger : MonoBehaviour
{
    public TimelineAsset timeline;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CinematicManager.Instance.Play(timeline);
        Destroy(gameObject);
    }
}
