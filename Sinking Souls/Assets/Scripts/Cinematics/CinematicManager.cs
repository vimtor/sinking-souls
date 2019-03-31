using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinematicManager : MonoBehaviour
{
    private PlayableDirector director;

    #region SINGLETON
    public static CinematicManager Instance { get; private set; }

    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void StartCinematic(TimelineAsset playable)
    {
		director.Play(playable);
	}
}
