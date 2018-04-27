using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State {

    float wait = 0;

    public override void PreUpdate(AI ai)
    {
        wait = 0;
        ai.animator.SetBool("run", false);
    }

    public override void Update(AI ai)
    {
        if (ai.aI_Type == AI_Type.Enemy)
            ((EnemyAI)ai).FindNearestTarget();

        if (wait >= ai.idleDuration)
        {
            ai.stateMachine.ChangeState("move");
        }

        wait += Time.deltaTime;
    }

    public override void Exit(AI ai)
    {
        
    }
}
