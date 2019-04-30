using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChallengeState {
    public bool winned;
    public bool onGoing;
}

public abstract class Challenge : MonoBehaviour {

    public bool started;
    public bool done;
    public bool updateAlways;
    ChallengeState state;

    public ChallengeState newState(bool onGoing, bool winned = false) {
        ChallengeState state;
        state.onGoing = onGoing;
        if (onGoing == true) {
            state.winned = false;
            return state;
        }
        else {
            state.winned = winned;
            return state;
        }
    }

    // Use this for initialization
    void Start () {
        LevelStart();
        state.onGoing = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (!done) {

            if (!started && GameController.instance.currentRoom.gameObject == gameObject) {//call Initialize if the player just got inside the room
                                                                                           //better make it when teh doors close
                Initialize();
                started = true;
            }
            if (started) {//if started call Update every frame
                state = StartedUpdate();
                if (state.onGoing == false) {
                    if (state.winned) {
                        Win();
                    }
                    else {
                        Loose();
                    }
                    started = false;
                    done = true;
                }
            }

            if (updateAlways) AlwaysUpdate();//sometimes it can be nedded a update that calls all the time ven when the player is not inside the room

        }

    }


    /// <summary>
    /// Function called when the player enters the room
    /// </summary>
    public abstract void Initialize();

    /// <summary>
    /// Function called at the beggining of the level
    /// </summary>
    public abstract void LevelStart();

    /// <summary>
    /// Function called when teh player is inside the room,
    /// <para>return a ChallengeState containing if the challeng is OnGoing, succesfully finished or failed finished</para>
    /// <para>on the return if OnGoing is false the challeng will finish (succesfully finished or failed have to be true then).</para>
    /// <para><para>!!! use newState() to return the state !!!</para></para>
    /// </summary>
    public abstract ChallengeState StartedUpdate();

    /// <summary>
    /// Fill this function in case you need something to be called all the time even when the player is not in the room
    /// </summary>
    public abstract void AlwaysUpdate();

    /// <summary>
    /// function called when the ChallengState returns a winn
    /// </summary>
    public abstract void Win();

    /// <summary>
    /// function called when the ChallengState returns a failed
    /// </summary>
    public abstract void Loose();
}
