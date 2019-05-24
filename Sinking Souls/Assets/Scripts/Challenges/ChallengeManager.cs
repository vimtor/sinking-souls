using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour {
    [System.Serializable]
    public enum Challenges {
        Test,
        KillAllNoDamage,
        TimeChallenge,
        None,
        NoBreakChallenge,// if you create a new one under here please
        DontUseThisOne
    }

    public GameObject UIMessage;

    public Challenges[] possibleChallenges;
    public float[] chance;

    [Header("Time Challenge objects")]
    public GameObject timeDisplay;
    public Gradient colorGradient;

    //////////////////////////////////////////////////////////////////////////public bool alwaysUpdate;
    public KeyValuePair<float, float> test;
	// Use this for initialization
	void Start () {
        UIMessage = GameController.instance.GetComponent<GameController>().ChestContentUI;
        Challenges possibleChallenge = Challenges.DontUseThisOne;
        do {
            int i = Random.Range(0, possibleChallenges.Length);
            Challenges pc = possibleChallenges[i];
            float val = Random.Range(0, 100f);
            if (val < chance[i]) possibleChallenge = pc;
            
        } while (possibleChallenge == Challenges.DontUseThisOne);


        switch (possibleChallenge) {
            case Challenges.Test:
            gameObject.AddComponent<TestChallenge>();
            GetComponent<TestChallenge>().updateAlways = false;
            GetComponent<TestChallenge>().challengeName = "Test Challenge";
            GetComponent<TestChallenge>().Message =UIMessage;
                break;
            case Challenges.KillAllNoDamage:
            gameObject.AddComponent<NoDamageCHallenge>();
            GetComponent<NoDamageCHallenge>().updateAlways = false;
            GetComponent<NoDamageCHallenge>().challengeName = "Take no damage!";
            GetComponent<NoDamageCHallenge>().Message = UIMessage;


                break;
            case Challenges.TimeChallenge:
            gameObject.AddComponent<TimeChallenge>();
            GetComponent<TimeChallenge>().updateAlways = false;
            GetComponent<TimeChallenge>().timeDisplay = timeDisplay;
            GetComponent<TimeChallenge>().colorGradient = colorGradient;
            GetComponent<TimeChallenge>().challengeName = "Kill them fast!";
            GetComponent<TimeChallenge>().Message = UIMessage;

                break;
            case Challenges.NoBreakChallenge:
            gameObject.AddComponent<NoBreakChallenge>();
            GetComponent<NoBreakChallenge>().updateAlways = false;
            GetComponent<NoBreakChallenge>().challengeName = "Don't break anything!";
            GetComponent<NoBreakChallenge>().Message = UIMessage;

                break;
            case Challenges.None:
            break;
            case Challenges.DontUseThisOne:
            break;
        }
        Destroy(this);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
