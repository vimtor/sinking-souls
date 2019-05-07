using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour {
    public enum Challenges {
        Test,
        KillAllNoDamage,
        None,
        DontUseThisOne
    }
    public List<KeyValuePair<Challenges, float>> possibleChallenges;
    public bool alwaysUpdate;
	// Use this for initialization
	void Start () {
        Challenges possibleChallenge = Challenges.DontUseThisOne;
        do {
            KeyValuePair<Challenges, float> pc = possibleChallenges[Random.Range(0, possibleChallenges.Count)];
            if (Random.Range(0, 100f) < pc.Value) possibleChallenge = pc.Key;
            
        } while (possibleChallenge == Challenges.DontUseThisOne);
       
        
        switch (possibleChallenge) {
            case Challenges.Test:
                gameObject.AddComponent<TestChallenge>();
                GetComponent<TestChallenge>().updateAlways = alwaysUpdate;
            break;
            case Challenges.KillAllNoDamage:
                gameObject.AddComponent<TestChallenge>();
                GetComponent<TestChallenge>().updateAlways = alwaysUpdate;
            break;

        }
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
