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

    }

    public override ChallengeState StartedUpdate()
    {
        if (initialLife > player.Health){

            return newState(false, false);//stop challenge and loose
        }

        if (!EnemiesAlive()) return newState(false, true);// stop challenge and win

        return newState(true);// nothing
    }

    public override string Win()
    {
        float auxMaxHealth = player.MaxHealth + GameController.instance.extraLife;
        if (player.Health < auxMaxHealth)
        {
            player.Heal((auxMaxHealth / 100) * 20); //Hela 20 % when win
            return "20% life heal";

        }
        else
        {
            GameController.instance.AddSouls(20);
            return "20 Souls";

        }
    }
}
