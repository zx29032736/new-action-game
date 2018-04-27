using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour {

    public string ID;

    public Dictionary<string, State> states = new Dictionary<string, State>();

    public State currentState;

    public AI myAI;

    //public StateMachine(string id)
    //{
    //    ID = id;
    //    myAI = GetComponent<AI>();
    //    AddState("idle", new IdleState());
    //}

    public void SetUpMachine(string id, AI ai)
    {
        ID = id;
        myAI = GetComponent<AI>();
        AddState("idle", new IdleState());
        AddState("attack", new AttackState());
        AddState("move", new MoveState());
        AddState("findingTarget", new FindingTargetState());
        AddState("deathState", new DeathState());
        AddState("stayState", new StayState());
    }

    public void AddState(string id, State state)
    {
        if (!states.ContainsKey(id))
            states.Add(id, state);
    }

    public void RemoveState(string id)
    {
        if (states.ContainsKey(id))
            states.Remove(id);
    }

    public void ChangeState(string id)
    {
        if (myAI == null || !states.ContainsKey(id))
            return;

        if (currentState != null)
            states[id].Exit(myAI);

        currentState = states[id];
        states[id].PreUpdate(myAI);
    }

    public void UpdateState()
    {
        if (currentState != null)
            currentState.Update(myAI);
    }
}
