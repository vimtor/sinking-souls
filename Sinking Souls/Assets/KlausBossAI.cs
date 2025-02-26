﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KlausBossAI : MonoBehaviour {

    
    [Header ("GENERAL:")]
    public bool attack = false;
    public float life = 100;

    [Header("Prefabs")]
    GameObject[] swords;
    public GameObject swordPrefab;
    public GameObject bossHealthbar;

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
    public float bigVelocityOffset;

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
    public bool started = false;

    private void Start() {
        started = false;
    }

    public void SetupAI()
    {
        randomVal = Random.Range(cadency.x, cadency.y);

        swords = new GameObject[6];
        restHoverOffset = new float[6];
        for (int i = 0; i < 6; i++)
        {
            swords[i] = Instantiate(swordPrefab);
            swords[i].transform.position = restPositions[i].transform.position;
            swords[i].transform.rotation = restPositions[i].transform.rotation;
            swords[i].transform.GetChild(0).GetComponent<WeaponHolder>().owner = gameObject;
            restHoverOffset[i] = Random.Range(0.7f, 1.3f);
        }
        gameObject.GetComponent<AIController>().SetupAI();
        var healthbar = Instantiate(bossHealthbar, GameObject.Find("Canvas").transform);
        healthbar.GetComponent<KlausBossHealthbar>().klaus = this;
        started = true;
    }

    private float acumulatedRotation = 0;
    Vector3 forward = Vector3.up;
    private float sweptInitialCounter;
    void swept() {


        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().dead)
            {
                swords[i].GetComponent<SwordBehaviour>().attack = true;
                if (sweptInitialCounter < sweptInitialWait)
                {        ///get in place
                    if (forward == Vector3.up) forward = Vector3.Cross(transform.forward, Vector3.up);
                    targget[i] = gameObject.transform.position + Vector3.up*1.5f + (firstDistance * forward.normalized) + forward.normalized * swordOffset * i;
                }
                else
                { ///rotate the stick
                    forward = Quaternion.Euler(new Vector3(0, swepingSpeed * Time.deltaTime, 0)) * forward;
                    targget[i] = gameObject.transform.position + Vector3.up * 1.5f + (firstDistance * forward.normalized) + forward.normalized * swordOffset * i;
                }
                ////move acording to targget///////////////////
                float speed = flyingSpeed;

                if ((targget[i] - swords[i].transform.position).magnitude < 1)
                {
                    speed = flyingSpeed * 5;
                }

                swords[i].GetComponent<Rigidbody>().velocity =//do not reuse, edited
                            (targget[i] - swords[i].transform.position).normalized *
                            (speed + firstDistance + swordOffset * i) *//(speed * i * (1 + (i/100))
                            ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                swords[i].transform.forward = targget[i] - swords[i].transform.position;


                if (acumulatedRotation < 90)
                {
                    acumulatedRotation += sweptRotationSpeed * Time.deltaTime;
                }
                swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);

                if (sweptInitialCounter < sweptInitialWait)
                {////calculate forwards depending on fase 
                    if ((targget[i] - swords[i].transform.position).magnitude < 1)
                    {///fase 1
                        swords[i].transform.forward = ((swords[i].transform.forward * (targget[i] - swords[i].transform.position).magnitude) + (forward * (1 - (targget[i] - swords[i].transform.position).magnitude)));
                        swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);
                    }
                }
                else
                {///fase 2
                    swords[i].transform.forward = forward;
                    swords[i].transform.Rotate(new Vector3(0, 0, 1), acumulatedRotation);

                }
                ////////////////////////////////////////////////
            }
        }

        ///end attack
        if(sweptInitialCounter >= rotationTime + sweptInitialWait) {
            acumulatedRotation = 0;
            forward = Vector3.up;
            sweptInitialCounter = 0;
            for (int i = 0; i < 6; i++) {
                if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                swords[i].GetComponent<SwordBehaviour>().attack = false;

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
                        if(GameController.instance.player.GetComponent<Rigidbody>().velocity.magnitude >= 0.1f) targget[i] = gameObject.transform.position + (((GameController.instance.player.transform.position + GameController.instance.player.transform.forward * bigVelocityOffset) - gameObject.transform.position).normalized * startingPos.x) + Vector3.up * startingPos.y;
                        else targget[i] = gameObject.transform.position + (((GameController.instance.player.transform.position) - gameObject.transform.position).normalized * startingPos.x) + Vector3.up * startingPos.y;
                        
                            float speed = flyingSpeed;

                        if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                            speed = flyingSpeed * 5;
                        }

                        swords[i].GetComponent<Rigidbody>().velocity =
                                    (targget[i] - swords[i].transform.position).normalized *
                                    speed * ((swords[i].transform.position - targget[i]).magnitude / relationAtenuation);

                        swords[i].transform.forward = targget[i] - swords[i].transform.position;

                        if ((targget[i] - swords[i].transform.position).magnitude < 1) {
                            if (GameController.instance.player.GetComponent<Rigidbody>().velocity.magnitude >= 0.1f) swords[i].transform.forward = new Vector3(((GameController.instance.player.transform.position + GameController.instance.player.transform.forward * bigVelocityOffset) - swords[i].transform.position).x, 0, ((GameController.instance.player.transform.position + GameController.instance.player.transform.forward * bigVelocityOffset) - swords[i].transform.position).z);//up
                            else swords[i].transform.forward = new Vector3(((GameController.instance.player.transform.position) - swords[i].transform.position).x, 0, ((GameController.instance.player.transform.position) - swords[i].transform.position).z);//up
                            swords[i].transform.Rotate(new Vector3(1, 0, 0), -90);
                        }
                    }
                }

                swords[selected].transform.localScale += new Vector3(Time.deltaTime * 1.2f, Time.deltaTime , Time.deltaTime) * activeSwords;
                swords[selected].GetComponent<SwordBehaviour>().AttackObject.GetComponent<WeaponHolder>().holder.damage = 40 * (1 + activeSwords / 10);

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

     
    public enum attacks { ORBIT, ARROW, SWEPT, BIG}
    public attacks currentAttack;
    private float betwinAttackCounter = 0;
    private bool allDeadDamage = false;
    private float bugedAttackCounter = 0;

    public float checkBuggedTime = 10;
    public float checkBuggedTimeOrbit = 20;
    private float BuggedTime;

    private void Update()
    {
        if (started)
        { 
            if (Input.GetKeyDown(KeyCode.F10)) life = -200;


            if (life <= 0)
            {
                GetComponent<Enemy>().Health = 0;
                for (int i = 0; i < 6; i++)
                {
                    swords[i].GetComponent<SwordBehaviour>().dead = true;

                }
                ApplicationManager.Instance.FinishGame();
            }
            else
            {
                GetComponent<Enemy>().Health = 1000000000;
            }
            ///calculate life;
            for (int i = 0; i < 6; i++)
            {
                if (swords[i].GetComponent<SwordBehaviour>().justDied())
                {
                    life -= 2;

                }
            }
            if (allDead() && !allDeadDamage)
            {
                life -= 13;
                allDeadDamage = true;
                if (life <= 0)
                {
                    for (int i = 0; i < 6; i++) giveGravity(i);
                }
            }

            if (attack)
            {
                BuggedTime = checkBuggedTime;
                switch (currentAttack)
                {
                    case attacks.ORBIT:
                        BuggedTime = checkBuggedTimeOrbit;
                        orbitAttack();
                        break;
                    case attacks.ARROW:
                        arrowAttack();
                        break;
                    case attacks.SWEPT:
                        swept();
                        break;
                    case attacks.BIG:
                        bigAttack();
                        break;
                }
                betwinAttackCounter = 0;
                if (bugedAttackCounter >= BuggedTime)
                {
                    attack = false;
                    foreach (GameObject sw in swords) sw.GetComponent<SwordBehaviour>().resetSword();

                }
                bugedAttackCounter += Time.deltaTime;

            }
            else
            {///when not attacking 
                bugedAttackCounter = 0;
                if (!allDead())
                {
                    rest();

                    for (int i = 0; i < 6; i++)
                    {
                        if (!swords[i].GetComponent<SwordBehaviour>().dead) swords[i].GetComponent<SwordBehaviour>().resetSword();
                    }

                    if (betwinAttackCounter >= 2)
                    {///wait X seconds and choos new attack and attack

                        currentAttack = chooseNewAttack();

                        //currentAttack = (attacks)(((int)currentAttack + 1)%4 );
                        attack = true;
                    }
                    betwinAttackCounter += Time.deltaTime;
                }
                else
                {
                    reviveAllDead(2);///if all dead revive them 
                }

            }
        }
    }

    float lastSoldAttack;
    public float maxSoldInterval = 3;
    public int alive;
    public float distance;
    public float provability;

    attacks chooseNewAttack() {
        alive = AliveSwords();
        distance = distanceToPlayer();
        attacks returnAttack = attacks.ORBIT;
        provability = Random.Range(0f, 1f);

        if (lastSoldAttack >= maxSoldInterval) {                                        ///Last sold 
            if (alive > 1) {
                if (distance > alive * 2.6f || distance < 2f) returnAttack = attacks.ORBIT;  /// im far so ORBIT
                else {

                    if (provability <= 0.5 + GameController.instance.player.GetComponent<Player>().map(distance, 2, alive * 2.6f, 0.4f, 0)) returnAttack = attacks.BIG;
                    else returnAttack = attacks.ORBIT;
                }

            }
            else {
                returnAttack = attacks.ORBIT;

            }
            lastSoldAttack = 0;
        }
        else {                                                                          ///normal
            lastSoldAttack++;
            returnAttack = attacks.ARROW;
            if (alive > 1) {
                if (distance > 20) {/// too far for swept
                                    ///only arrow or orbit 
                    if (provability <= 0.7f) returnAttack = attacks.ARROW;
                    else {
                        returnAttack = attacks.ORBIT;
                        lastSoldAttack = 0;
                    }
                        
                }
                else if (distance > alive * 2.6f) {/// to far for big
                    if (provability <= 0.40f) returnAttack = attacks.ARROW;
                    else if (provability <= 0.800f) returnAttack = attacks.SWEPT;
                    else{
                        returnAttack = attacks.ORBIT;
                        lastSoldAttack = 0;
                    }
                }
                else if (distance > 2) {/// big
                    if(provability <= 0.22f){
                        returnAttack = attacks.BIG;
                        lastSoldAttack = 0;
                    }
                    else if(provability <= 0.30f) {
                        returnAttack = attacks.ORBIT;
                        lastSoldAttack = 0;
                    }
                    else if(provability <= 0.65f) returnAttack = attacks.SWEPT;
                    else returnAttack = attacks.ARROW;
                }
                else {///to close 
                    if (provability <= 0.7f) returnAttack = attacks.SWEPT;
                    else {
                        returnAttack = attacks.ORBIT;
                        lastSoldAttack = 0;
                    }
                }

            }
            else {///1 alive so orbit or arrow
                if (provability < 0.5f){
                    returnAttack = attacks.ORBIT;
                    lastSoldAttack = 0;
                }
                else returnAttack = attacks.ARROW;

            }
        }
        return returnAttack;
    }
    
    float distanceToPlayer() {
        return (GameController.instance.player.transform.position - transform.position).magnitude;
    }

    int AliveSwords() {
        int counter = 0;
        for(int i = 0; i < 6; i++){
            if (!swords[i].GetComponent<SwordBehaviour>().dead) counter++;
        }
        return counter;
    }

    bool AllInactive() {
        for (int i = 0; i < 6; i++) {
            if (!swords[i].GetComponent<SwordBehaviour>().inactive) {
                return false;
            }
        }
        return true;
    }

    bool allDead()
    {
        for (int i = 0; i < 6; i++)
        {
            if (!swords[i].GetComponent<SwordBehaviour>().dead)
            {
                return false;
            }
        }
        return true;
    }

    void reviveAllDead(float t) {
        if (allDead())
        {
            StartCoroutine(reviveDead(t));
        }
    }
    IEnumerator reviveDead(float t)
    {
        yield return new WaitForSeconds(t);
        for (int i = 0; i < 6; i++)
        {
            swords[i].GetComponent<SwordBehaviour>().resetSword();
            swords[i].GetComponent<SwordBehaviour>().revive();
        }
        allDeadDamage = false;
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
