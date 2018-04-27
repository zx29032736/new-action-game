using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayState : State {

    public override void PreUpdate(AI ai)
    {
        ai.animator.SetBool("run", false);
        ((NPC)ai).LookAtPlayer();
        //ai.transform.LookAt()
    }
}
