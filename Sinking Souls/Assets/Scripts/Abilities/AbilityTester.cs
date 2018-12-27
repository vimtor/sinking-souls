using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTester : MonoBehaviour {

    public Ability ability;
    public int delay = 2;

    private float time = 0;

	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time >= delay)
        {
            time = 0;
            ability.Use(this.gameObject, this.transform);
        }
	}
}
