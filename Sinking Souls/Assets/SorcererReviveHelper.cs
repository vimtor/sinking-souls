using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererReviveHelper : MonoBehaviour {

    public GameObject melee;
    public GameObject distance;
    public GameObject instantiated;
    public GameObject particles;
    public enum EnemyType{
        Mele,
        Distance
        }

    
    public void revive(EnemyType en, Vector3 pos) {
        if (GetComponent<Enemy>().dead) return;
        GameObject f = Instantiate(particles, null, false);
        Destroy(f, 5);
        f.transform.position = pos;
        StartCoroutine(WaitAndSpawn(en, pos));
        
    }
    IEnumerator WaitAndSpawn(EnemyType en, Vector3 pos) {
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject enemy = null;
        switch (en) {
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