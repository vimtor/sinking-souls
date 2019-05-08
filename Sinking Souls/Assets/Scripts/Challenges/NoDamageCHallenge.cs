using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDamageCHallenge : Challenge
{

    public override void AlwaysUpdate(){}
    public override void LevelStart() {}
    public override void Loose() {}


    public bool Damaged;
    public float initialLife;
    Player player;
    public override void Initialize(){
        Damaged = false;
        initialLife = GameController.instance.player.GetComponent<Player>().Health;
        player = GameController.instance.player.GetComponent<Player>();
        Debug.Log("//////////////////////////////////////////////////////////////No Damage Challenge Started");

    }

    public override ChallengeState StartedUpdate()
    {
        if (initialLife > player.Health){
            Debug.Log("//////////////////////////////////////////////////////////////Lost");
            return newState(false, false);//stop challenge and loose
        }

        if (!EnemiesAlive()) return newState(false, true);// stop challenge and win

        return newState(true);// nothing
    }

    public override void Win()
    {
        float auxMaxHealth = player.MaxHealth + GameController.instance.extraLife;
        if (player.Health < auxMaxHealth)
        {
            player.Heal((auxMaxHealth / 100) * 20); //Hela 20 % when win
            Debug.Log("//////////////////////////////////////////////////////////////Winned and added life");

        }
        else
        {
            GameController.instance.AddSouls(20);
            Debug.Log("//////////////////////////////////////////////////////////////Winned and added Souls");

        }
    }
}
