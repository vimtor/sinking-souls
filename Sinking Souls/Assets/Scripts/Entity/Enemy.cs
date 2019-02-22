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
        if (m_Health <= 0) Die();
        if (dead)
        {
            deathCounter += Time.deltaTime;
            transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_height", GameController.instance.player.GetComponent<Player>().map(deathCounter, 0, deathDuration, 1, 0));
        }
       
    }

    private void Die()
    {
        if (!dead)
        {
            Animator.SetTrigger("Die");
            StartCoroutine(WaitToDie(deathDuration));
            GameController.instance.player.GetComponent<Player>().lockedEnemy = null;
            GameController.instance.player.GetComponent<Player>().ResetMovement();
            dead = true;
            GetComponent<AIController>().aiActive = false;
            //GetComponent<CapsuleCollider>().enabled = false;
            GameController.instance.RunSouls += m_Souls;
            Debug.Log("Dead");
        }

    }

    IEnumerator WaitToDie(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Destroy");

        for(int i = 0; i< GameController.instance.roomEnemies.Count; i++) 
        {
            if (GameController.instance.roomEnemies[i] == gameObject)
            {
                GameController.instance.roomEnemies.Remove(GameController.instance.roomEnemies[i]);
            }
        }
        Debug.Log("Destroy 2");
        Destroy(gameObject);
    }
}
