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

    [Header("Arrow attack")]
    public float arrowInitialWait;
    public float maxForward;

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
    public float endDelay;

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





    private float initialArrowCounter = 0;
    void arrowAttack() {
       
        ///1- Position in formation
        if(initialArrowCounter < arrowInitialWait) {
            //Set place
            Vector3 forward = (GameController.instance.player.transform.position - gameObject.transform.position).normalized;           
            int forwardOffset = 1;
            float lateralOffset = forwardOffset /2f ;

            for(int i =0; i<6; i+=2) {
                targget[i] = (gameObject.transform.position + Vector3.up) + (forward * maxForward / 3 * (4 - forwardOffset)) + Vector3.Cross(forward, Vector3.up) * (lateralOffset / 2f);
                targget[i+1] = (gameObject.transform.position + Vector3.up) + (forward * maxForward / 3 * (4 - forwardOffset)) + Vector3.Cross(Vector3.up, forward) * (lateralOffset / 2f);
                forwardOffset++;
                lateralOffset = forwardOffset;
            }

            //Move
            for (int i = 0; i< 6; i++) {
                swords[i].GetComponent<Rigidbody>().velocity =
                            (targget[i] - swords[i].transform.position).normalized *
                            flyingSpeed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);
                swords[i].transform.forward = targget[i] - swords[i].transform.position;


                if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                    swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                }
            }
        }

        ///2- 
    }


    private float initialCounter = 0;
    private float cadencyCounter = 0;
    private float backCounter = 0;
    private int attacker = -1;
    private List<int> selecteds = new List<int>();
    private Vector3[] targget = new Vector3[6];
    private float endCounter = 0;



    void orbitAttack() {
        Vector3 originalDirecion = (GameController.instance.player.transform.position + Vector3.forward * swordOrbitDistance + Vector3.up * swordOrbitHeight) - GameController.instance.player.transform.position;
        originalDirecion = Quaternion.Euler(new Vector3(0, rotationOffset, 0)) * originalDirecion;

        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                if (!selecteds.Contains(i) && !swords[i].GetComponent<SwordBehaviour>().stopLooking) {
                    targget[i] = GameController.instance.player.transform.position + Quaternion.Euler(new Vector3(0, (360 / 6) * i, 0)) * originalDirecion;
                    swords[i].GetComponent<Rigidbody>().velocity =
                            (targget[i] - swords[i].transform.position).normalized *
                            flyingSpeed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                    swords[i].GetComponent<SwordBehaviour>().lookPlayer();
                }
            }
            else if (!selecteds.Contains(i)) selecteds.Add(i);
            
            
        }
        rotationOffset = ((rotationOffset + rotationSpeed * Time.deltaTime) % 360);

        if (initialCounter > initialWait && selecteds.Count < 6) {
            //select a new one;
            if (cadencyCounter > cadency.x) {
                int index;
                do {
                    index = Random.Range(0, 6);
                } while (selecteds.Contains(index));
                if (selecteds.Count == 0) {
                    selecteds.Add(index);
                    cadencyCounter = 0;
                }

                if (swords[selecteds[selecteds.Count - 1]].GetComponent<SwordBehaviour>().inactive) {
                    selecteds.Add(index);
                    cadencyCounter = 0;
                }



            }

            //launch
            if (selecteds.Count != 0) {
                swords[selecteds[selecteds.Count - 1]].GetComponent<SwordBehaviour>().launch();

            }
            cadencyCounter += Time.deltaTime;
        }


        initialCounter += Time.deltaTime;

        if (selecteds.Count >= 6) {
            if(swords[selecteds[selecteds.Count - 1]].GetComponent<SwordBehaviour>().inactive) {

                if(endCounter >= endDelay) {
                    selecteds = new List<int>();
                    attack = false;
                    for (int i = 0; i < 6; i++) {
                        if(!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                    }
                    initialCounter = 0;
                    cadencyCounter = 0;
                    backCounter = 0;
                    attacker = -1;
                    endCounter = 0;
                }

                endCounter += Time.deltaTime;

                
            }

        }
        
    }



    private void Update() {
        if (attack) arrowAttack();
        else rest();

        if(!attack)reviveAllDead();

    }

    void reviveAllDead() {
        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                return;
            }
        }
        for (int i = 0; i < 6; i++) {
            swords[i].GetComponent<SwordBehaviour>().resetSword();
            swords[i].GetComponent<SwordBehaviour>().revive();
        }
    }
    void rest() {
        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                targget[i] = restPositions[i].transform.position + Vector3.up * Mathf.Sin(Time.time * restHoverSpeed * restHoverOffset[i]) * restHoverAmount;

                swords[i].GetComponent<Rigidbody>().velocity =
                            (targget[i] - swords[i].transform.position).normalized *
                            flyingSpeed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);
                swords[i].transform.forward = targget[i] - swords[i].transform.position;
                if((targget[i] - swords[i].transform.position).magnitude < 1) {
                    swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (restPositions[i].transform.forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                }
               // swords[i].transform.forward = restPositions[i].transform.forward; //Quaternion.LookRotation( restPositions[i].transform.forward + new Vector3(0,0,Mathf.Sin(Time.deltaTime * restHoverSpeed)* restHoverAmount));

            }
        }
    }
}
