using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_Type { Enemy, NPC }
public enum MoveMode { Patrolling , Drifting}
[System.Serializable]
public class MoveStateProperties
{
    public MoveMode moveMode = MoveMode.Patrolling;
    //partolling
    public GameObject[] patrollingWayPoint = new GameObject[3];
    [System.NonSerialized]
    public GameObject headToPoint;
    public int wayStep = 0;
    //
    //drifting
    public Transform center = null;
    [System.NonSerialized]
    public Vector3 randomDestination;
    [System.NonSerialized]
    public float distanceToCenter = Mathf.Infinity;
    public float maxDistanceToCenter = 10;
    //
    public float moveSpeed = 4;
    public float moveDuration = 3;
}

public class AI : MonoBehaviour
{
    [System.NonSerialized]
    public StateMachine stateMachine;
    [System.NonSerialized]
    public CharacterController characterController;
    [System.NonSerialized]
    public Animator animator;
    [System.NonSerialized]
    public EnemyStat stat;

    [HideInInspector]
    public AI_Type aI_Type;
    public new string name = "ai";
    //idle
    public float idleDuration = 2f;
    //move
    public MoveStateProperties moveStateProperties;

    private void Start()
    {
        //characterController = GetComponent<CharacterController>();
        //stateMachine = GetComponent<StateMachine>();
        //animator = GetComponent<Animator>();
        //stat = GetComponent<EnemyStat>();
        //stateMachine.SetUpMachine(gameObject.name, this);
        //stateMachine.ChangeState("idle");
        //Init();
    }

    public void SwitchFirstState(string stateName)
    {
        stateMachine.SetUpMachine(gameObject.name, this);
        stateMachine.ChangeState(stateName);
    }

    public void UseGravity()
    {
        characterController.SimpleMove(new Vector3(0, -9.81f, 0));
    }

  
}
