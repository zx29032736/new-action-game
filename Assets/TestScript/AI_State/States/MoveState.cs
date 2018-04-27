using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State {

    float wait = 0;
    Vector3 destination = Vector3.zero;
    public override void PreUpdate(AI ai)
    {
        wait = 0;
        ai.animator.SetBool("run", true);

        if (ai.moveStateProperties.moveMode == MoveMode.Patrolling)
            WaypointStep(ai);
        else if (ai.moveStateProperties.moveMode == MoveMode.Drifting)
            WayDritfing(ai);
        else
            Debug.LogError("Create new mode to ai.");
    }

    public override void Update(AI ai)
    {
        if (ai.aI_Type == AI_Type.Enemy)
            ((EnemyAI)ai).FindNearestTarget();

        //destination = ai.moveStateProperties.randomDestination;
        //destination.y = ai.transform.position.y;
        //ai.transform.LookAt(destination);

        if (ai.moveStateProperties.moveMode == MoveMode.Patrolling)
            Patrolling(ai);
        else if (ai.moveStateProperties.moveMode == MoveMode.Drifting)
            Drifting(ai);
        else
            Debug.LogError("Create new mode to ai.");

        float distance = (ai.transform.position - destination).magnitude;
        if (distance <= 0.1f)
        {
            ai.stateMachine.ChangeState("idle");
        }

        wait += Time.deltaTime;

        if (wait >= ai.moveStateProperties.moveDuration)
            ai.stateMachine.ChangeState("idle");
    }

    public override void Exit(AI ai)
    {
        base.Exit(ai);
    }

    void Drifting(AI ai)
    {
        Debug.DrawLine(ai.transform.position, ai.moveStateProperties.randomDestination, Color.red);

        Vector3 forward = ai.transform.TransformDirection(Vector3.forward);
        ai.characterController.Move(forward * ai.moveStateProperties.moveSpeed * Time.deltaTime);

    }

    void WayDritfing(AI ai)
    {
        if(ai.moveStateProperties.center == null)
        {
            ai.stateMachine.ChangeState("idle");
            Debug.LogError(" Please set up ai's drifting cneter! ");
            return;
        }

        ai.moveStateProperties.distanceToCenter = (ai.transform.position - ai.moveStateProperties.center.position).magnitude;
        if (ai.moveStateProperties.distanceToCenter >= ai.moveStateProperties.maxDistanceToCenter)
        {
            ai.moveStateProperties.randomDestination = ai.moveStateProperties.center.position;
        }
        else
        {
            float rngX = Random.Range(-ai.moveStateProperties.maxDistanceToCenter, ai.moveStateProperties.maxDistanceToCenter) + ai.moveStateProperties.center.position.x;
            float rngZ = Random.Range(-ai.moveStateProperties.maxDistanceToCenter, ai.moveStateProperties.maxDistanceToCenter) + ai.moveStateProperties.center.position.z;
            ai.moveStateProperties.randomDestination = new Vector3(rngX, ai.transform.position.y, rngZ);
        }

        destination = ai.moveStateProperties.randomDestination;
        destination.y = ai.transform.position.y;
        ai.transform.LookAt(destination);
    }

    void Patrolling(AI ai)
    {

        //Vector3 destination = ai.moveStateProperties.headToPoint.transform.position;
        //destination.y = ai.transform.position.y;
        //ai.transform.LookAt(destination);

        Vector3 forward = ai.transform.TransformDirection(Vector3.forward);
        ai.characterController.Move(forward * ai.moveStateProperties.moveSpeed * Time.deltaTime);

        //float distance = (ai.transform.position - ai.moveStateProperties.headToPoint.transform.position).magnitude;
        //if (distance <= 0.5f)
        //{
        //    ai.stateMachine.ChangeState("idle");
        //}

        //wait += Time.deltaTime;

        //if (wait >= ai.idleDuration)
        //    ai.stateMachine.ChangeState("idle");
    }

    void WaypointStep(AI ai)
    {
        if(ai.moveStateProperties.patrollingWayPoint.Length == 0 || ai.moveStateProperties.wayStep > ai.moveStateProperties.patrollingWayPoint.Length  - 1 || ai.moveStateProperties.patrollingWayPoint[ai.moveStateProperties.wayStep] == null)
        {
            ai.stateMachine.ChangeState("idle");
            Debug.LogError("Please set up patrollingWayPoint!");
            return;
        }

        ai.moveStateProperties.headToPoint = ai.moveStateProperties.patrollingWayPoint[ai.moveStateProperties.wayStep];

        if (ai.moveStateProperties.wayStep >= ai.moveStateProperties.patrollingWayPoint.Length -1)
            ai.moveStateProperties.wayStep = 0;
        else
            ai.moveStateProperties.wayStep++;

        destination = ai.moveStateProperties.headToPoint.transform.position;
        destination.y = ai.transform.position.y;
        ai.transform.LookAt(destination);
    }
}
