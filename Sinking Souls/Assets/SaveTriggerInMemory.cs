using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTriggerInMemory : MonoBehaviour {

    private void Awake()
    {
        if (GameController.instance.visitedTavern)
        {
            Destroy(gameObject);
            Debug.Log("Hola?");
            GameController.instance.player.GetComponent<Player>().Resume();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameController.instance.visitedTavern = true;
    }
}
