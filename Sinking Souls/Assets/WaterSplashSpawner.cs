using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashSpawner : MonoBehaviour {
    public int amount;
    public Vector2 radiusOfAction;
    public GameObject watterSplash;

   [HideInInspector] public float delay;
   [HideInInspector] public float range;
   [HideInInspector] public bool pickRandomPosition;

    void Start () {
		for(int i = 0; i<amount; i++) {
            Vector3 pos = new Vector3(Random.Range(-100,100), 0, Random.Range(-100, 100));
            Debug.Log("Random Position: " + pos);
            Vector3 direction = (pos - GetComponent<AbilityHolder>().owner.GetComponent<AIController>().player.transform.position).normalized;
            Debug.Log("Player Position: " + GetComponent<AbilityHolder>().owner.GetComponent<AIController>().player.transform.position);
            pos = GetComponent<AbilityHolder>().owner.GetComponent<AIController>().player.transform.position + direction * Random.Range(radiusOfAction.x, radiusOfAction.y);
            Debug.Log("final Position: " + pos);

            GameObject instatiated = Instantiate(watterSplash);
            instatiated.transform.position = pos;

            instatiated.AddComponent<AbilityHolder>().holder = GetComponent<AbilityHolder>().holder;
            instatiated.GetComponent<AbilityHolder>().owner = GetComponent<AbilityHolder>().owner;

            instatiated.GetComponent<WaterSplashBehaviour>().delay = delay;
            instatiated.GetComponent<WaterSplashBehaviour>().range = range;
            instatiated.GetComponent<WaterSplashBehaviour>().pickRandomPosition = pickRandomPosition;
            Destroy(instatiated, delay + 0.5f);
        }
        Destroy(gameObject);
	}
	
}
