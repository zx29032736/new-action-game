using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindingTargetState : State {

    public override void PreUpdate(AI ai)
    {
        //base.PreUpdate(ai);
        //Debug.Log(ai.followTarget.name + " is finded !");
    }

    public override void Update(AI ai)
    {
        EnemyAI enemyAI = (EnemyAI)ai;

        if(enemyAI.distanceToTarget <= enemyAI.approachDistance)
        {
            enemyAI.stateMachine.ChangeState("attack");
            //attack
        }
        else if(enemyAI.distanceToTarget >= enemyAI.lostSight)
        {
            //return move
            enemyAI.TargetFleed();
            enemyAI.stateMachine.ChangeState("move");
        }
        else
        {
            Vector3 forward = enemyAI.transform.TransformDirection(Vector3.forward);
            enemyAI.characterController.Move(forward * ai.moveStateProperties.moveSpeed * Time.deltaTime);
            enemyAI.transform.LookAt(enemyAI.followTarget.transform);
            enemyAI.transform.eulerAngles = new Vector3(0, enemyAI.transform.eulerAngles.y, enemyAI.transform.eulerAngles.z);
            enemyAI.animator.SetBool("run", true);
        }


    }

    public override void Exit(AI ai)
    {
        base.Exit(ai);
    }

}
