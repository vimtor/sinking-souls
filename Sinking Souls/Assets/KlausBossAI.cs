using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlausBossAI : MonoBehaviour {

    GameObject[] swords;
    public GameObject swordPrefab;
    public GameObject[] restPositions;

    public float restHoverSpeed;
    public float restHoverAmount;
    private float[] restHoverOffset;

    public float swordOrbitDistance;
    public float swordOrbitHeight;

    public float flyingSpeed;

    private void Start() {
        swords = new GameObject[6];
        restHoverOffset = new float[6];
        for(int i = 0; i<6; i++) {
            swords[i] = Instantiate(swordPrefab);
            swords[i].transform.position = restPositions[i].transform.position;
            swords[i].transform.rotation = restPositions[i].transform.rotation;
            swords[i].transform.GetChild(0).GetComponent<WeaponHolder>().owner = gameObject;
            restHoverOffset[i] = Random.Range(0.7f, 1.3f);
        }
    }

    void orbitAttack() {

        Vector3 originalDirecion = (GameController.instance.player.transform.position + GameController.instance.player.transform.forward * swordOrbitDistance + Vector3.up * swordOrbitHeight) - GameController.instance.player.transform.position;

        for (int i = 0; i< 6; i++) {
            Vector3 targget = (GameController.instance.player.transform.position + Quaternion.Euler(new Vector3(0, (360 / 6) * i, 0)) * originalDirecion);
            swords[i].GetComponent<Rigidbody>().velocity =
                (targget - transform.position).normalized * flyingSpeed *
                Mathf.Clamp01((targget - transform.position).magnitude);

            swords[i].GetComponent<SwordBehaviour>().lookPlayer();
            Debug.Log(i);
        }

    }

    public bool attack = false;
    private void Update() {
        //rest();
        if (attack) orbitAttack();
        else rest();
    }

    void rest() {
        for (int i = 0; i < 6; i++) {
            swords[i].transform.position = restPositions[i].transform.position + Vector3.up * Mathf.Sin(Time.time * restHoverSpeed * restHoverOffset[i]) * restHoverAmount ;
            swords[i].transform.rotation = restPositions[i].transform.rotation; //Quaternion.LookRotation( restPositions[i].transform.forward + new Vector3(0,0,Mathf.Sin(Time.deltaTime * restHoverSpeed)* restHoverAmount));
        }
    }
}
