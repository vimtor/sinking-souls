using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity{
    enum State {
        STATE_IDLE,
        STATE_ATTACK,
        STATE_ATTACK2,
        STATE_ATTACK3,
        STATE_MOVEMENT,
        STATE_ABILITY,
        STATE_DASH
    };

    public float dashSpeed;
    private State state;
    public Animator animator;

    private Dictionary<Enemy.EnemyType, int> inventory;
    public AbilitySO dash;
    public AbilitySO ability;

    public void HandleInput() { }
    public void Ability() { }
    public void Dash() { }
    public void Attack() { }
}
