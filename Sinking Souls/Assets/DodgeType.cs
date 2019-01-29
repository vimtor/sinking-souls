using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeType : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "Perfect" && other.tag=="Player")
        {
            other.GetComponent<Player>().Dodge = Player.DodgeType.PERFECT;
        }
        else if(gameObject.tag == "Normal" && other.tag == "Player")
        {
            other.GetComponent<Player>().Dodge = Player.DodgeType.NORMAL;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") other.GetComponent<Player>().Dodge = Player.DodgeType.NONE;
    }
}
