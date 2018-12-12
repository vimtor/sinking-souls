using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterSplashBehaviour : MonoBehaviour {

    [HideInInspector] public float delay;
    public bool pickRandomPosition = false;
    public float range = 20;
    private ParticleSystem particles;
    private Vector3 playerPosition;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();
        if (!pickRandomPosition) playerPosition = GameController.instance.player.transform.position;
        else RandomNavmeshLocation(GameController.instance.currentRoom.transform.position);
        particles.Stop();
        StartCoroutine(Splash());
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator Splash() {
        yield return new WaitForSecondsRealtime(delay);
        transform.position = playerPosition;
        particles.Play();
    }

    public Vector3 RandomNavmeshLocation(Vector3 center) {
        bool found = false;
        Vector3 finalPosition = Vector3.zero;

        while (!found) {
            Vector3 randomDirection = Random.insideUnitSphere * range;
            randomDirection += center;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, range, 1)) {
                finalPosition = hit.position;
                found = true;
            }
        }

        return finalPosition;
    }


}
