using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererReviveHelper : MonoBehaviour {

    public GameObject melee;
    public GameObject distance;
    public GameObject instantiated;
    public GameObject particles;//explosion
    public GameObject magic;//trail
    private GameObject castedMagic;
    [Header("Augmented")]
    public bool augmented;
    public GameObject augmentedMagic;//trails
    public GameObject augmentedParticles;//explosions
    public float closeDistance;
    public float time;

    private float timer = 0;
    bool playerCloseDuringTime() {
        if (Vector3.Distance(GameController.instance.player.transform.position, gameObject.transform.position) < closeDistance) timer += Time.deltaTime;
        else timer = 0;
        if((timer >= time)) {
            timer = 0;
            return true;
        }
        return false;
    }


    public void Update() {
        if (augmented) {

            if (playerCloseDuringTime()) {
                GameObject holder = null;
                //gets a mele if available and if not a archer
                foreach (GameObject enem in GameController.instance.roomEnemies) {
                    if (enem.GetComponent<AIController>().aiActive) {
                        if (gameObject.name.Contains("Melee")) {
                            holder = enem;
                            break;
                        }
                        else {
                            if (enem != gameObject) {
                                holder = enem;
                            }
                        }
                    }
                }

                if (holder == null) return;
                //spawn effects and tp 
                GameObject f = Instantiate(augmentedParticles, null, false);
                Destroy(f, 5);
                GetComponent<Entity>().noDamage = true;
                holder.GetComponent<Entity>().noDamage = true;
                f.transform.position = gameObject.transform.position; //mine
                StartCoroutine(EnableDamage(holder));
                GameObject efe = Instantiate(augmentedParticles, null, false);
                Destroy(efe, 5);
                efe.transform.position = holder.transform.position; //other

                StartCoroutine(SwapPositions(holder));

                augmentedMagic.transform.position = gameObject.transform.position + Vector3.up * 2;
                GameObject augmCastedMagic = Instantiate(augmentedMagic, null, false);
                augmCastedMagic.GetComponent<castedMagicBehaviour>().chase(holder.transform.position, gameObject, true);///from me to toher

                augmentedMagic.transform.position = holder.transform.position + Vector3.up * 2;
                GameObject augmCastedMagic1 = Instantiate(augmentedMagic, null, false);
                augmCastedMagic1.GetComponent<castedMagicBehaviour>().chase(gameObject.transform.position, gameObject, true);///crida a 3
            }
        }
    }


    public enum EnemyType{
        Mele,
        Distance
    }

    private EnemyType type;

    IEnumerator EnableDamage(GameObject other)
    {
        yield return new WaitForSecondsRealtime(1.4f);
        GetComponent<Entity>().noDamage = false;
        other.GetComponent<Entity>().noDamage = false;
    }

    public void revive(EnemyType en, Vector3 pos) { //1
        if (GetComponent<Enemy>().dead) return;
        
            GetComponent<Animator>().SetTrigger("ReviveSomeone");
            AudioManager.Instance.Play("Revive");
            StartCoroutine(CastMagic(en, pos));
     

    }
    IEnumerator SwapPositions(GameObject holder) {
        yield return new WaitForSecondsRealtime(0.5f);
        Vector3 help = holder.transform.position;
        holder.transform.position = transform.position;
        transform.position = help;
    }

    IEnumerator CastMagic(EnemyType en, Vector3 pos)//2
    {
        yield return new WaitForSecondsRealtime(1.5f);
        magic.transform.position = gameObject.transform.position + Vector3.up * 2;
        castedMagic = Instantiate(magic, null, false);
        castedMagic.GetComponent<castedMagicBehaviour>().chase(pos, gameObject);///crida a 3
        type = en;
    }
    
    public void reviveNow(Vector3 pos)//3
    {
        GameObject f = Instantiate(particles, null, false);
        Destroy(f, 5);
        f.transform.position = pos;
        StartCoroutine(WaitAndSpawn(pos));

    }

    IEnumerator WaitAndSpawn( Vector3 pos) {//4
        yield return new WaitForSecondsRealtime(0.2f);
        GameObject enemy = null;
        switch (type) {
            case EnemyType.Mele:
            melee.transform.position = Vector3.zero;
            enemy = Instantiate(melee, GameController.instance.currentRoom.transform, false);
            Debug.Log("Created a melee by magic");
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