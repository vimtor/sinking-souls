using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagePilarsBehaviour : MonoBehaviour {

    public GameObject pilar;
    public int count;
    public float range;
    public Vector2 spawnTimes;
    public float duration;
    private bool stop = false;
    private float time = 0;

    private void Start() {
        for (int i = 0; i < count; i++) StartCoroutine(spawnPillar(Random.Range(spawnTimes.x, spawnTimes.y)));
    }

    // Update is called once per frame
    void Update () {
        transform.position = GameController.instance.player.transform.position;
        

        if (!stop && time >= duration) {
            stop = true;
            Destroy(gameObject);
        }

        time += Time.deltaTime;
	}

    IEnumerator spawnPillar(float t) {
        yield return new WaitForSeconds(t);
        
        GameObject p = Instantiate(pilar);
        p.AddComponent<AbilityHolder>().holder = GetComponent<AbilityHolder>().holder;
        p.GetComponent<AbilityHolder>().owner = GetComponent<AbilityHolder>().owner;
        Vector3 direction = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100)).normalized * Random.Range(0,range);
        p.transform.position = transform.position + direction;
        if(!stop)StartCoroutine(spawnPillar(Random.Range(spawnTimes.x, spawnTimes.y)));
    }

}
