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
    [Header("Swept attack")]
    public float firstDistance;
    public float swordOffset;
    public float sweptRotationSpeed;
    public float sweptInitialWait;
    public float swepingSpeed;
    public float rotationTime;

    [Header("Big attack")]
    public Vector2 startingPos;

    [Header("Arrow attack")]
    public float arrowInitialWait;
    public float maxForward;
    public float speedMultiplier;
    private float forwardTime;

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

    private float acumulatedRotation = 0;
    Vector3 forward = Vector3.up;
    private float sweptInitialCounter;
    void swept() {


        for (int i = 0; i < 6; i++) {
            swords[i].GetComponent<SwordBehaviour>().attack = true;
            if (sweptInitialCounter < sweptInitialWait) {        ///get in place
                if (forward == Vector3.up) forward = Vector3.Cross(transform.forward, Vector3.up);
                    targget[i] = gameObject.transform.position + Vector3.up + (firstDistance * forward.normalized) + forward.normalized * swordOffset * i;
            }
            else { ///rotate the stick
                forward = Quaternion.Euler(new Vector3(0, swepingSpeed * Time.deltaTime, 0)) * forward;
                targget[i] = gameObject.transform.position + Vector3.up + (firstDistance * forward.normalized) + forward.normalized * swordOffset * i;
            }
            ////move acording to targget///////////////////
            float speed = flyingSpeed;

            if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                speed = flyingSpeed * 5;
            }

            swords[i].GetComponent<Rigidbody>().velocity =//do not reuse, edited
                        (targget[i] - swords[i].transform.position).normalized *
                        (speed + firstDistance + swordOffset * i) *//(speed * i * (1 + (i/100))
                        ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

            swords[i].transform.forward = targget[i] - swords[i].transform.position;


            if (acumulatedRotation < 90) {
                acumulatedRotation += sweptRotationSpeed * Time.deltaTime;
            }
            swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);

            if (sweptInitialCounter < sweptInitialWait) {////calculate forwards depending on fase 
                if ((targget[i] - swords[i].transform.position).magnitude < 1) {///fase 1
                    swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                    swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);
                }
            }
            else {///fase 2
                swords[i].transform.forward = forward;
                swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);

            }
            ////////////////////////////////////////////////
        }

        ///end attack
        if(sweptInitialCounter >= rotationTime + sweptInitialWait) {
            acumulatedRotation = 0;
            forward = Vector3.up;
            sweptInitialCounter = 0;
            for (int i = 0; i < 6; i++) {
                if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                else swords[i].GetComponent<SwordBehaviour>().attack = false;

            }
            attack = false;
        }

        sweptInitialCounter += Time.deltaTime;    
    }


    int selected = -1;
    float startingCounter;
    float activeSwords = 0;
    int timesAttaked;
    float otherTimesCounter = 1;
    void bigAttack() {///not call if only 1 sword;
        if (selected == -1) {
            for(int i = 0; i < 6; i++) {
                if (!swords[i].GetComponent<SwordBehaviour>().inactive) {
                    selected = i;
                    activeSwords++;
                }
                
            }
        }
        else {
            if (startingCounter < 1) {///get to starting position + size
                for (int i = 0; i < 6; i++) {
                    if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                        targget[i] = gameObject.transform.position + ((GameController.instance.player.transform.position - gameObject.transform.position).normalized * startingPos.x) + Vector3.up * startingPos.y;

                        float speed = flyingSpeed;

                        if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                            speed = flyingSpeed * 5;
                        }

                        swords[i].GetComponent<Rigidbody>().velocity =
                                    (targget[i] - swords[i].transform.position).normalized *
                                    speed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                        swords[i].transform.forward = targget[i] - swords[i].transform.position;

                        if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                            swords[i].transform.forward = new Vector3((GameController.instance.player.transform.position - swords[i].transform.position).x, 0, (GameController.instance.player.transform.position - swords[i].transform.position).z);//up
                            swords[i].transform.Rotate(new Vector3(1, 0, 0), -90);
                        }
                    }
                }

                swords[selected].transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * activeSwords;
                swords[selected].GetComponent<SwordBehaviour>().AttackObject.GetComponent<WeaponHolder>().holder.damage = swords[selected].GetComponent<SwordBehaviour>().originalDamage * 1 + activeSwords / 10;

            }
            else {///falling
                swords[selected].GetComponent<Rigidbody>().velocity = Vector3.zero;
                for (int i = 0; i < 6; i++) {
                    if (i != selected && !swords[i].GetComponent<SwordBehaviour>().dead) {
                        swords[i].transform.position = swords[selected].transform.position;
                        swords[i].transform.rotation = swords[selected].transform.rotation;
                    }
                    else {
                        swords[selected].GetComponent<SwordBehaviour>().giantAttack(true);
                    }
                }
                
            }
        }
        if (swords[selected].GetComponent<SwordBehaviour>().inactive) {///goingUp

            if (!swords[selected].GetComponent<SwordBehaviour>().dead) swords[selected].transform.Rotate(new Vector3(1, 0, 0), -Time.deltaTime * swords[selected].GetComponent<SwordBehaviour>().rotationSpeed / 3);

            if (swords[selected].transform.localScale.x > 1) {

                swords[selected].transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * activeSwords;
            }

            if(swords[selected].transform.localScale.x <= 1) {
                if (swords[selected].GetComponent<SwordBehaviour>().dead) {
                    attack = false;
                    for (int i = 0; i < 6; i++) {
                        if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                        else swords[i].GetComponent<SwordBehaviour>().bigAttack = false;
                    }
                    startingCounter = 0;
                    swords[selected].transform.localScale = new Vector3(1, 1, 1);

                    activeSwords = 0;
                    giveGravity(selected);
                    selected = -1;

                }
            }else if ((swords[selected].transform.forward - Vector3.up).magnitude <= 0.05f) {///gotten to the upper part
                {///finish attack
                    attack = false;
                    for (int i = 0; i < 6; i++) if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                    startingCounter = 0;
                    swords[selected].transform.localScale = new Vector3(1,1,1);
                    selected = -1;
                    activeSwords = 0;
                }

            }
        }




        startingCounter += Time.deltaTime;
        otherTimesCounter += Time.deltaTime;
    }

    private float initialArrowCounter = 0;
    void arrowAttack() {

        ///1- Position in formation
        if (initialArrowCounter < arrowInitialWait) {
            //Set place
            Vector3 forward = (GameController.instance.player.transform.position - gameObject.transform.position).normalized;
            int forwardOffset = 1;
            float lateralOffset = forwardOffset / 2f;

            for (int i = 0; i < 6; i += 2) {
                if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                    targget[i] = (gameObject.transform.position + Vector3.up) + (forward * maxForward / 3 * (4 - forwardOffset)) + Vector3.Cross(Vector3.up, forward) * (lateralOffset / 2f);
                    targget[i + 1] = (gameObject.transform.position + Vector3.up) + (forward * maxForward / 3 * (4 - forwardOffset)) + Vector3.Cross(forward, Vector3.up) * (lateralOffset / 2f);
                    forwardOffset++;
                    lateralOffset = forwardOffset;
                }
            }

            //Move
            for (int i = 0; i < 6; i++) {
                if (!swords[i].GetComponent<SwordBehaviour>().dead) {
                    swords[i].transform.forward = targget[i] - swords[i].transform.position;
                    float speed = flyingSpeed;

                    if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                        swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                        speed = flyingSpeed * 5;
                    }

                    swords[i].GetComponent<Rigidbody>().velocity =
                                (targget[i] - swords[i].transform.position).normalized *
                                speed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);
                }
            }
            forwardTime = ((GameController.instance.player.transform.position + (GameController.instance.player.transform.position - transform.position).normalized) - transform.position ).magnitude * 1.5f / (swords[0].GetComponent<SwordBehaviour>().forwardSpeed * speedMultiplier);

        }
        ///2- 
        else {
            for (int i = 0; i < 6; i++) if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().forwadLaunch(forwardTime, speedMultiplier);
            if (AllInactive()) {
                attack = false;
                for (int i = 0; i < 6; i++) {
                    if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                }
                initialArrowCounter = 0;
            }

        }



        initialArrowCounter += Time.deltaTime;
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
        if (attack) swept();
        
        else rest();

        if(!attack)reviveAllDead();

    }

    bool AllInactive() {
        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().inactive) {
                return false;
            }
        }
        return true;
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

                float speed = flyingSpeed;

                if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                    speed = flyingSpeed * 5;
                }

                swords[i].GetComponent<Rigidbody>().velocity =
                            (targget[i] - swords[i].transform.position).normalized *
                            speed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                swords[i].transform.forward = targget[i] - swords[i].transform.position;

                if((targget[i] - swords[i].transform.position).magnitude < 1) {
                    swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (restPositions[i].transform.forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                }
               // swords[i].transform.forward = restPositions[i].transform.forward; //Quaternion.LookRotation( restPositions[i].transform.forward + new Vector3(0,0,Mathf.Sin(Time.deltaTime * restHoverSpeed)* restHoverAmount));

            }
        }
    }

    void giveGravity(int i) {
        swords[i].GetComponent<Rigidbody>().freezeRotation = false;
        swords[i].GetComponent<Rigidbody>().useGravity = true;


        StartCoroutine(stopGravity(1, i));
        StartCoroutine(rotateAtFall(1, i));

    }

    IEnumerator rotateAtFall(float t, int i) {
        yield return new WaitForEndOfFrame();
        t -= Time.deltaTime;
        swords[i].transform.Rotate(new Vector3(1,0,0),Time.deltaTime * 50);
        if(t > 0) StartCoroutine(rotateAtFall(t, i));
    }

    IEnumerator stopGravity(float t, int i) {
        yield return new WaitForSeconds(t);
        swords[i].GetComponent<Rigidbody>().freezeRotation = true;
        swords[i].GetComponent<Rigidbody>().useGravity = false;
    }
}
