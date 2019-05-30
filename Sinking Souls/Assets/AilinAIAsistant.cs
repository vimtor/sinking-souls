using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilinAIAsistant : MonoBehaviour {
    AIController controller;


   

    [Header("RageMode")]
    public float rageTime;
    public float rageDuration;

    [Space(5)]
    [Header("Teleport options")]
    public float tpTime;
    public float tpDistance;

    [Space(5)]
    [Header("Debug options")]
    public bool rageMode = false;
    public bool firstAttack = false;

    [HideInInspector] public float rageCounter = 0;
    [HideInInspector] public float rageDurationCounter = 0;
    [HideInInspector] public float tpCounter = 0;

    private float lastLife;
    public GameObject bossHealthbar;

    void Start () {
        controller = GetComponent<AIController>();
        lastLife = controller.gameObject.GetComponent<Entity>().Health;

        var healthbar = Instantiate(bossHealthbar, GameObject.Find("Canvas").transform);
        healthbar.GetComponent<AilinBossHealthbar>().ailin = GetComponent<Enemy>();


        AudioManager.Instance.PlayFade("KlausTheme", 5, 0);
        AudioManager.Instance.Fade("TritonTheme", 2);
    }
	
	
	void Update () {
        //TP
        if (Vector3.Distance(controller.player.transform.position, transform.position) < tpDistance) {
            tpCounter += Time.deltaTime;
        }
        else tpCounter = 0;

        //Rage
        if (!Hited()) {
            rageCounter += Time.deltaTime;
        }
        else
            rageCounter = 0;

        if(rageCounter> rageTime) {
            rageMode = true;
            rageCounter = 0;
        }
        if (rageMode) {
            rageDurationCounter += Time.deltaTime;
            if(rageDurationCounter > rageDuration) {
                rageMode = false;
                rageDurationCounter = 0;
                firstAttack = false;
            }
        }

        if (GetComponent<Entity>().Health < 0)
        {
            AudioManager.Instance.PlayFade("TritonTheme", 5, 0);
            AudioManager.Instance.Fade("KlausTheme", 2);
        }
	}

    bool Hited() {

        if(lastLife != controller.gameObject.GetComponent<Entity>().Health) {
            lastLife = controller.gameObject.GetComponent<Entity>().Health;
            return true;
        }
        return false;
    }
}
