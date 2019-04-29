using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {

    private AIController m_AIController;
    private bool dash;
    public int m_Souls;
    public float deathDuration = 1f;
    private float dashSpeed;

    [HideInInspector] public Ability[] abilities;



    private void Start()
    {
        OnStart();

        m_AIController = GetComponent<AIController>();
        abilities = GetComponent<Enemy>().Abilities;
    }

    private float deathCounter = 0;

    private void Update()
    {
        if (transform.position.y < -20) Health = -10;

        if (m_Health <= 0) Die();

        if (CurrentModifierState[ModifierState.TOXIC] > 0) transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Poison", 1);
        else transform.GetChild(1).GetComponent<Renderer>().material.SetInt("_Poison", 0);

        if (dead)
        {
            deathCounter += Time.deltaTime;
            transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_height", GameController.instance.player.GetComponent<Player>().map(deathCounter, 0, deathDuration, 1, 0));
        }
       
    }
    public GameObject allie = null;
    private void Die()
    {
        if (!dead)
        {

            StartCoroutine(WaitToDie(deathDuration));
            if (Animator != null) Animator.SetTrigger("Die");
            else {
                StartCoroutine(fallToDie(1));
                GetComponent<BoxCollider>().isTrigger = true;
                GetComponent<Rigidbody>().useGravity = false;
            }
            GameController.instance.player.GetComponent<Player>().lockedEnemy = null;
            GameController.instance.player.GetComponent<Player>().ResetMovement();
            dead = true;
            GetComponent<AIController>().aiActive = false;
            //GetComponent<CapsuleCollider>().enabled = false;
            GameController.instance.AddSouls(m_Souls);
            if (allie != null) {
                GameObject all = Instantiate(allie);
                all.GetComponent<AIController>().SetupAI();
                all.transform.position = transform.position;
            }
        }

    }

    IEnumerator fallToDie(float time) {
        yield return new WaitForEndOfFrame();
        transform.Rotate(Vector3.left, 90 * Time.deltaTime, Space.Self);
        if(time -Time.deltaTime > 0) {
            StartCoroutine(fallToDie(time - Time.deltaTime));
        }
    }
    public bool DestroyOnDeath = true;
    IEnumerator WaitToDie(float time)
    {
        for (int i = 0; i < GameController.instance.roomEnemies.Count; i++) {
            if (GameController.instance.roomEnemies[i] == gameObject) {
                GameController.instance.roomEnemies.Remove(GameController.instance.roomEnemies[i]);
            }
        }
        gameObject.layer = 20;
        yield return new WaitForSeconds(time);
        if(DestroyOnDeath)Destroy(gameObject);
    }
}
