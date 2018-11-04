using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashBehaviour : MonoBehaviour {

    [HideInInspector] public float delay;

    private ParticleSystem particles;
    private Vector3 playerPosition;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();
        playerPosition = GameController.instance.player.transform.position;
        particles.Stop();
        StartCoroutine(Splash());
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator Splash() {
        yield return new WaitForSecondsRealtime(delay);
        transform.position = playerPosition;
        particles.Play();
    }

}
