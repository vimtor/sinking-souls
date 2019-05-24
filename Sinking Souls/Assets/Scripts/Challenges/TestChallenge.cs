using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChallenge : Challenge {
    
    public override void AlwaysUpdate() {
        Debug.Log("Always Updating on room: " + gameObject.name);
    }

    public override void Initialize() {
        Debug.Log("I Got initialized");

    }

    public override void LevelStart() {
        
        Debug.Log("On Level Start");
    }

    public override void Loose() {
        Debug.Log("Lost");
    }

    public override ChallengeState StartedUpdate() {
        Debug.Log("Update, B to loose, M to win");
        if (Input.GetKeyDown(KeyCode.B)) return newState(false, false);
        if (Input.GetKeyDown(KeyCode.M)) return newState(false, true);
        return newState(true);
    }

    public override string Win() {
        Debug.Log("Winned");
        return "the fuck";

    }
}
