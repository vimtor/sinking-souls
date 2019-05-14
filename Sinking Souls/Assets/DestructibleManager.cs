using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleManager : MonoBehaviour {
    List<GameObject> amphoraDestructibles = new List<GameObject>();
    int spawnedAmphs = 0;
	// Use this for initialization
	void Start () {

        for(int i = 0; i< transform.childCount; i++){
            Debug.Log(gameObject.name + " child: " + transform.GetChild(i).gameObject.name);
            if(transform.GetChild(i).gameObject.GetComponent<DestructibleProp>()) {
                Debug.Log("HAHAHAHAHAHAHAHHA");
                amphoraDestructibles.Add(transform.GetChild(i).gameObject);
                amphoraDestructibles[amphoraDestructibles.Count-1].SetActive(false);
                if (spawnedAmphs < GameController.instance.AmphoraMinPerRoom){
                    amphoraDestructibles[amphoraDestructibles.Count - 1].SetActive(true);
                    spawnedAmphs++;
                }
                else if(spawnedAmphs >= GameController.instance.AmphoraMaxPerRoom){

                }
                else{
                    if(Random.Range(0f, 100f) < 20) {
                        amphoraDestructibles[amphoraDestructibles.Count - 1].SetActive(true);
                        spawnedAmphs++;
                    }
                }
            }
        }
	}
}
