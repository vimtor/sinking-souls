using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaterSplashBehaviour : MonoBehaviour {

    public ParticleSystem ribbles;
    public ParticleSystem waterSplash;
    public ParticleSystem[] splashEffects;

    [HideInInspector] public float delay;
    [HideInInspector] public bool pickRandomPosition;
    [HideInInspector] public float range;

    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        // Set the start delay for the main splash.
        var mainSplash = waterSplash.main;
        mainSplash.startDelay = delay;

        // Do the same for the effects.
        foreach(var effect in splashEffects)
        {
            var effectMain = effect.main;
            effectMain.startDelay = delay;
        }

        StartCoroutine(Splash());
    }

    private IEnumerator Splash() {
        // Set initial position.
        if (!pickRandomPosition) transform.position = GameController.instance.player.transform.position;
        else transform.position = RandomNavmeshLocation(GameController.instance.currentRoom.transform.position);

        // After the delay enable the hit box and destroy the game object.
        yield return new WaitForSecondsRealtime(delay);
        boxCollider.enabled = true;
        Destroy(gameObject, 0.3f);
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
