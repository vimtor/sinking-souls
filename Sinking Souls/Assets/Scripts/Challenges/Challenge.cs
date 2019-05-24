using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ChallengeState {
    public bool winned;
    public bool onGoing;
}

public abstract class Challenge : MonoBehaviour {

    public bool started;
    public bool done;
    public bool updateAlways;
    ChallengeState state;
    public string challengeName;
    private bool showMessage;
    private GameObject UIHandler;
    public GameObject Message;

    public float apearingSpeed = 10;

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
	
    void ShowCHallengeUI() {
        GameController.instance.player.GetComponent<ShowChallengeUI>().Show(challengeName);
    }

	// Update is called once per frame
	void Update () {

        if (!done) {

            if (!started && GameController.instance.currentRoom.gameObject == gameObject) {//call Initialize if the player just got inside the room
                                                                                           //better make it when teh doors close
                ///desplay message
                if (EnemiesAlive())
                {                                                                           
                    Initialize();
                    ShowCHallengeUI();
                    started = true;
                }
            }
            if (started) {//if started call Update every frame
                state = StartedUpdate();
                if (state.onGoing == false) {
                    if (state.winned) {
                        string winMss = Win();
                        showMessage = true;
                        UIHandler = Instantiate(Message, GameObject.Find("Canvas").transform, false);
                        UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge reward: " + "<color=#00ff48><size=40>" + winMss +"</size></color>";
                    }
                    else {
                        Loose();
                        showMessage = true;
                        UIHandler = Instantiate(Message, GameObject.Find("Canvas").transform, false);
                        UIHandler.GetComponentInChildren<TextMeshProUGUI>().text = "Challenge <color=#ff0000><size=40>lost" + "</size></color>";

                    }
                    started = false;
                    done = true;
                }
            }

            if (updateAlways) AlwaysUpdate();//sometimes it can be nedded a update that calls all the time ven when the player is not inside the room

        }

        if (!showMessage)
        {
            UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, UIHandler.GetComponent<Image>().color.a - apearingSpeed * Time.deltaTime);
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.a - apearingSpeed * Time.deltaTime);
            if (UIHandler.GetComponent<Image>().color.a <= 0)
            {
                UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, 0);
                UIHandler.GetComponentInChildren<TextMeshPro>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshPro>().color.r, UIHandler.GetComponentInChildren<TextMeshPro>().color.g, UIHandler.GetComponentInChildren<TextMeshPro>().color.b, 0);
            }

        }
        else
        {
            UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, UIHandler.GetComponent<Image>().color.a + apearingSpeed * Time.deltaTime);
            UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.a + apearingSpeed * Time.deltaTime);

            if (UIHandler.GetComponent<Image>().color.a >= 1)
            {
                UIHandler.GetComponent<Image>().color = new Vector4(UIHandler.GetComponent<Image>().color.r, UIHandler.GetComponent<Image>().color.g, UIHandler.GetComponent<Image>().color.b, 1);
                UIHandler.GetComponentInChildren<TextMeshProUGUI>().color = new Vector4(UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.r, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.g, UIHandler.GetComponentInChildren<TextMeshProUGUI>().color.b, 1);
                StartCoroutine(HideMessagito(2.2f));
            }

        }

    }

    IEnumerator HideMessagito(float t)
    {
        yield return new WaitForSecondsRealtime(t);
        showMessage = false;
        Destroy(UIHandler, 5);
    }

    public bool EnemiesAlive()
    {
        foreach (GameObject e in GameController.instance.roomEnemies)
        {
            if (e.GetComponent<AIController>().aiActive)
            {
                return true;
            }
        }

        return false;
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
    public abstract string Win();

    /// <summary>
    /// function called when the ChallengState returns a failed
    /// </summary>
    public abstract void Loose();
}
