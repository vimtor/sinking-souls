using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CinematicManager : MonoBehaviour
{
    public GameObject inGameInterface;
    public GameObject blackFrames;

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

        director.played += OnCinematicPlay;
        director.stopped += OnCinematicStop;
    }

    public void Play(TimelineAsset playable)
    {
        director.Play(playable);
        GameController.instance.player.SetActive(false);
    }

    private void OnCinematicPlay(PlayableDirector playable)
    {
        inGameInterface.SetActive(false);
        blackFrames.SetActive(true);
        GameController.instance.player.GetComponent<Player>().Stop();
    }

    private void OnCinematicStop(PlayableDirector playable)
    {
        inGameInterface.SetActive(true);
        blackFrames.SetActive(false);
        GameController.instance.player.SetActive(true);
        GameController.instance.player.GetComponent<Player>().Resume();
    }
}
