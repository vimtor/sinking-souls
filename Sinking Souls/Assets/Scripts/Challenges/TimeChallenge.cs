using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeChallenge : Challenge
{
    public override void AlwaysUpdate() { }

    private float t;
    [HideInInspector] public float maxTime = 16;
    public GameObject timeDisplay;
    private GameObject display;
    public Gradient colorGradient;
    private Vector3 originalScale;
    public override void Initialize()
    {
        t = 0;
        display = Instantiate(timeDisplay);
        display.transform.position = GameController.instance.currentRoom.transform.position + (Vector3.up * 2);
        originalScale = display.transform.localScale;
    }

    public override void LevelStart(){}

    public override void Loose(){}

    public override ChallengeState StartedUpdate()
    {
        if (t > maxTime)
        {
            Destroy(display);
            return newState(false, false);
        }
        if (!EnemiesAlive())
        {
            Debug.Log("PFDSOJKFSD:POFJKPSPODFJKSDPOIFJSP:OFIJA:PIFJDA:SPIF");
            //Destroy(display);
            return newState(false, true);
        }
        updateTimer();
        t += Time.deltaTime;
        return newState(true);
    }

    public override void Win()
    {
        GameController.instance.AddSouls(10);
    }

    public void updateTimer()
    {
        display.GetComponent<TextMeshPro>().text = ((int)(maxTime - t)).ToString();
        float transcursion = GameController.instance.player.GetComponent<Player>().map((int)(maxTime - t), maxTime, 0, 0, 1);
        display.GetComponent<TextMeshPro>().color = colorGradient.Evaluate(transcursion);
        if(t >= maxTime - 6) sizeController(t - ((int)t));
    }

    public void sizeController(float dt)
    {
        if (dt < 0.1f)
        {
            float aux = GameController.instance.player.GetComponent<Player>().map(dt,0, 0.1f, 1, 1.3f);
            display.transform.localScale = originalScale * aux;
        }
        else if(dt < 0.1f)
        {
            

        }
        else if( dt< 0.7f)
        {
            float aux = GameController.instance.player.GetComponent<Player>().map(dt, 0.2f, 0.7f, 1.3f, 1);
            display.transform.localScale = originalScale * aux;


        }
    }
}
