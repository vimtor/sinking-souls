using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlausBossAI : MonoBehaviour {

    
    [Header ("GENERAL:")]
    public bool attack = false;

    [Header("Prefabs")]
    GameObject[] swords;
    public GameObject swordPrefab;

    [Header ("Rest")]
    public GameObject[] restPositions;
    public float restHoverSpeed;
    public float restHoverAmount;
    private float[] restHoverOffset;


    [Header("ATTACKS:")]
    
    [Header("Orbit attack")]
    ///Orbit attack
    //Swords position
    public float swordOrbitDistance;
    public float swordOrbitHeight;
    public float rotationSpeed = 1;
    public float relationAtenuation;
    public float initialWait;
    public Vector2 cadency;
    public float backOffset;

    //Swords rotation
    private float rotationOffset = 0;
    public float flyingSpeed;

    private float randomVal;

    private void Start() {

        randomVal = Random.Range(cadency.x, cadency.y);

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


    private float initialCounter = 0;
    private float cadencyCounter = 0;
    private float backCounter = 0;
    private int attacker = -1;
    private List<int> selecteds = new List<int>();
    private Vector3[] targget = new Vector3[6];

    void orbitAttack() {
        Vector3 originalDirecion = (GameController.instance.player.transform.position + Vector3.forward * swordOrbitDistance + Vector3.up * swordOrbitHeight) - GameController.instance.player.transform.position;
        originalDirecion = Quaternion.Euler(new Vector3(0, rotationOffset, 0)) * originalDirecion;

        for (int i = 0; i < 6; i++) {
            if (!selecteds.Contains(i)) {
                targget[i] = GameController.instance.player.transform.position + Quaternion.Euler(new Vector3(0, (360 / 6) * i, 0)) * originalDirecion;
                swords[i].GetComponent<Rigidbody>().velocity =
                        (targget[i] - swords[i].transform.position).normalized *
                        flyingSpeed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                swords[i].GetComponent<SwordBehaviour>().lookPlayer();
            }
            
        }
        rotationOffset = ((rotationOffset + rotationSpeed * Time.deltaTime) % 360);

        if (initialCounter > initialWait && selecteds.Count < 6) {
            //select a new one;
            if (cadencyCounter > cadency.x) {
                int index;
                do {
                    index = Random.Range(0, 6);
                } while (selecteds.Contains(index));
                if (selecteds.Count == 0) selecteds.Add(index);

                if (swords[selecteds[selecteds.Count - 1]].GetComponent<SwordBehaviour>().inactive) selecteds.Add(index);

                cadencyCounter = 0;

            }

            //launch
            if (selecteds.Count != 0) {
                swords[selecteds[selecteds.Count - 1]].GetComponent<SwordBehaviour>().launch();

            }
            cadencyCounter += Time.deltaTime;
        }


        initialCounter += Time.deltaTime;

        if (selecteds.Count >= 6) {
            selecteds = new List<int>();
            attack = false;
        }
        
    }

    private void Update() {
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
