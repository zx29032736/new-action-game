using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStat {

    [System.NonSerialized]
    public EnemyAI myAI;

    [TextArea(1,3)]
    public string description;
    public Sprite icon;

    private void Start()
    {
        SetUpStat();
        myAI = GetComponent<EnemyAI>();
    }

    public override void Die()
    {
        base.Die();
        //myAI.stateMachine.ChangeState("deathState");
        myAI.Die();
    }

    public void RestEmenyStats()
    {
        if (isDead)
        {
            currentHealth = maxHealth;
            currentMana = Maxmana;
            isDead = false;
        }
    }
}
