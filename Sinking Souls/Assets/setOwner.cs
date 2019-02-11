using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setOwner : MonoBehaviour {
    bool done = false;
	// Use this for initialization
	void Start () {
        if (!done && GetComponent<AbilityHolder>())
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.AddComponent<AbilityHolder>().owner = GetComponent<AbilityHolder>().owner;
                transform.GetChild(i).gameObject.GetComponent<AbilityHolder>().holder = GetComponent<AbilityHolder>().holder;
            }

            done = true;
        }

    }



}
