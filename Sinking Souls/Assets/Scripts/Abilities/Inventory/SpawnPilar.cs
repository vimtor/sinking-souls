using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPilar : MonoBehaviour {

    public GameObject pilar;
    public float delay = 0.8f;
    private float count = 0;
    public float pilarLifespan = 1f;
    public float AuraLifespan = 0.8f;
    private GameObject actualPilar;
    private bool done = false;

    private void Start() {
        var main1 = GetComponent<ParticleSystem>().main;
        var main2 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        main1.startLifetime = main2.startLifetime = AuraLifespan;
        AudioManager.Instance.Play("AilinColumn");
    }

    void Update () {
        if (count >= delay && !done) {
            actualPilar = Instantiate(pilar);
            actualPilar.transform.position = transform.position;
            actualPilar.transform.parent = transform;
            var main = actualPilar.GetComponent<ParticleSystem>().main;
            main.startLifetime = pilarLifespan;
            Destroy(actualPilar, pilarLifespan);
            done = true;
            actualPilar.AddComponent<AbilityHolder>().holder = gameObject.GetComponent<AbilityHolder>().holder;
            actualPilar.GetComponent<AbilityHolder>().owner = gameObject.GetComponent<AbilityHolder>().owner;
            Destroy(gameObject, AuraLifespan);
        }
        count += Time.deltaTime;
	}
}
