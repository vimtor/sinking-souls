using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageWaveSpawner : MonoBehaviour {

    public GameObject wave;

    public float spawnInterval;
    public int count;

	// Use this for initialization
	void Start () {
		for(int i =0; i< count; i++) {
            StartCoroutine(Spawn(spawnInterval * i));
        }
        Destroy(gameObject, count * spawnInterval);

	}
	
    IEnumerator Spawn(float t) {
        yield return new WaitForSeconds(t);
        GameObject i = Instantiate(wave);
        i.AddComponent<AbilityHolder>().holder = GetComponent<AbilityHolder>().holder;
        i.GetComponent<AbilityHolder>().owner = GetComponent<AbilityHolder>().owner;
        i.GetComponent<ExpansiveWaveBehaviour>().damage = GetComponent<AbilityHolder>().holder.damage;
        i.transform.position = gameObject.transform.position + new Vector3(0,1,0);
    }
}
