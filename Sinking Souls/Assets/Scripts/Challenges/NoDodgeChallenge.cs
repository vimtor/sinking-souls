using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDodgeChallenge : Challenge
{
    public override void AlwaysUpdate() {}

    private Player player;
    public override void Initialize()
    {
        player = GameController.instance.player.GetComponent<Player>();
    }

    public override void LevelStart() {}

    public override void Loose()
    {
        throw new System.NotImplementedException();
    }

    public override ChallengeState StartedUpdate()
    {
        if (GameController.instance.player.GetComponent<Player>().m_PlayerState == Player.PlayerState.DASHING) return newState(false, false);
        if (EnemiesAlive()) return newState(false, true);
        return newState(true);
    }

    public override void Win()
    {
        float auxMaxHealth = player.MaxHealth + GameController.instance.extraLife;
        if (player.Health < auxMaxHealth)
        {
            player.Heal((auxMaxHealth / 100) * 20); //Hela 20 % when win
        }
        else
        {
            GameController.instance.AddSouls(20);
        }
    }
}
