using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveTrigger : MonoBehaviour {

    public string sphere;

    private void OnTriggerStay(Collider other)
    {
        switch (sphere)
        {
            case "interior":
                if (other.tag == "Player") transform.parent.GetComponent<ExpansiveWaveBehaviour>().interiorCollision = true;
                break;
            case "exterior":
                if (other.tag == "Player") {
                transform.parent.GetComponent<ExpansiveWaveBehaviour>().exteriorCollision = true;
                Debug.Log("Exterior true");
                }
                break;
        }    
    }
}
