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
        DontUseThisOne
    }

    public Challenges[] possibleChallenges;
    public float[] chance;

    [Header("Time Challenge objects")]
    public GameObject timeDisplay;
    public Gradient colorGradient;

    //////////////////////////////////////////////////////////////////////////public bool alwaysUpdate;
    public KeyValuePair<float, float> test;
	// Use this for initialization
	void Start () {
        Challenges possibleChallenge = Challenges.DontUseThisOne;
        do {
            int i = Random.Range(0, possibleChallenges.Length);
            Challenges pc = possibleChallenges[i];
            if (Random.Range(0, 100f) < chance[i]) possibleChallenge = pc;
            
        } while (possibleChallenge == Challenges.DontUseThisOne);
       
        
        switch (possibleChallenge) {
            case Challenges.Test:
                gameObject.AddComponent<TestChallenge>();
                GetComponent<TestChallenge>().updateAlways = false;
                GetComponent<TestChallenge>().challengeName = "Test Challenge";
            break;
            case Challenges.KillAllNoDamage:
                gameObject.AddComponent<NoDamageCHallenge>();
                GetComponent<NoDamageCHallenge>().updateAlways = false;
                GetComponent<NoDamageCHallenge>().challengeName = "Take no damage!";
                
            break;
            case Challenges.TimeChallenge:
                gameObject.AddComponent<TimeChallenge>();
                GetComponent<TimeChallenge>().updateAlways = false;
                GetComponent<TimeChallenge>().timeDisplay = timeDisplay;
                GetComponent<TimeChallenge>().colorGradient = colorGradient;
                GetComponent<TimeChallenge>().challengeName = "Kill them fast!";
                break;

        }
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
