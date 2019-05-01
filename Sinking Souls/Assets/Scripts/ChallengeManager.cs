using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : MonoBehaviour {
    public enum Challenges {
        Test,
        Other,
        SomeMore
    }
    public Challenges possibleChallenges;
    public bool alwaysUpdate;
	// Use this for initialization
	void Start () {
        switch (possibleChallenges) {
            case Challenges.Test:
                gameObject.AddComponent<TestChallenge>();
                GetComponent<TestChallenge>().updateAlways = alwaysUpdate;
            break;
            case Challenges.SomeMore:

            break;

        }
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
