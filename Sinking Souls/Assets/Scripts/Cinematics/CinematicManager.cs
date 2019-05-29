using System;
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

    private double lastTime = -1;
    private GameObject cutsceneWrapper;
    private bool reactivate;

    [HideInInspector] public bool isPlaying;

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
    }
    #endregion

    private void Start()
    {
        director = GetComponent<PlayableDirector>();

        director.played += OnCinematicPlay;
        director.stopped += OnCinematicStop;

        cutsceneWrapper = GameObject.FindGameObjectWithTag("CutsceneWrapper");
        cutsceneWrapper.SetActive(false);
    }

    private void Update()
    {
        if (reactivate)
        {
            reactivate = false;
            cutsceneWrapper.SetActive(true);
        }

        if (director.time == lastTime)
        {
            reactivate = true;
            cutsceneWrapper.SetActive(false);
        }

        lastTime = director.time;
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
        isPlaying = true;
    }

    private void OnCinematicStop(PlayableDirector playable)
    {
        inGameInterface.SetActive(true);
        blackFrames.SetActive(false);
        GameController.instance.player.SetActive(true);
        GameController.instance.player.GetComponent<Player>().Resume();
        isPlaying = false;
    }

    public void EmergencyStop()
    {
        inGameInterface.SetActive(true);
        blackFrames.SetActive(false);
        GameController.instance.player.SetActive(true);
        GameController.instance.player.GetComponent<Player>().Resume();
        isPlaying = false;
        cutsceneWrapper.SetActive(false);
    }
}
