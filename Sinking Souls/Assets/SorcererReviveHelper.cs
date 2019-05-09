using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererReviveHelper : MonoBehaviour {

    public GameObject melee;
    public GameObject distance;
    public GameObject instantiated;
    public GameObject particles;
    public GameObject magic;
    private GameObject castedMagic;
    public enum EnemyType{
        Mele,
        Distance
    }

    private EnemyType type;
    public void revive(EnemyType en, Vector3 pos) {
        if (GetComponent<Enemy>().dead) return;
        GetComponent<Animator>().SetTrigger("ReviveSomeone");

        StartCoroutine(CastMagic(en, pos));
    }
    IEnumerator CastMagic(EnemyType en, Vector3 pos)
    {
        yield return new WaitForSecondsRealtime(1.5f);
        magic.transform.position = gameObject.transform.position + Vector3.up * 2;
        castedMagic = Instantiate(magic, null, false);
        castedMagic.GetComponent<castedMagicBehaviour>().chase(pos, gameObject);
        type = en;
    }
    
    public void reviveNow(Vector3 pos)
    {
        GameObject f = Instantiate(particles, null, false);
        Destroy(f, 5);
        f.transform.position = pos;
        StartCoroutine(WaitAndSpawn(pos));
    }

    IEnumerator WaitAndSpawn( Vector3 pos) {
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject enemy = null;
        switch (type) {
            case EnemyType.Mele:
            melee.transform.position = Vector3.zero;
            enemy = Instantiate(melee, GameController.instance.currentRoom.transform, false);
            if (enemy.tag == "Enemy") GameController.instance.roomEnemies.Add(enemy);
            enemy.GetComponent<AIController>().SetupAI();
            enemy.transform.position = pos;
            enemy.transform.forward = GameController.instance.player.transform.position - enemy.transform.position;
            break;
            case EnemyType.Distance:
            distance.transform.position = Vector3.zero;
            enemy = Instantiate(distance, GameController.instance.currentRoom.transform, false);
            if (enemy.tag == "Enemy") GameController.instance.roomEnemies.Add(enemy);
            enemy.GetComponent<AIController>().SetupAI();
            enemy.transform.position = pos;
            enemy.transform.forward = GameController.instance.player.transform.position - enemy.transform.position;
            break;
        }
        enemy.GetComponent<Animator>().SetTrigger("Revive");
    }
}