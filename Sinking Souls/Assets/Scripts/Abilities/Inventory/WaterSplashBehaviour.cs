using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplashBehaviour : MonoBehaviour {

    [HideInInspector] public float delay = 0.0f;

    private ParticleSystem particles;

    private void Awake() {
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
        StartCoroutine(Splash());
    }

    private IEnumerator Splash() {
        yield return new WaitForSeconds(delay);
        GameObject player = GameController.instance.player;
        transform.position = player.transform.position;
        particles.Play();
    }

}
