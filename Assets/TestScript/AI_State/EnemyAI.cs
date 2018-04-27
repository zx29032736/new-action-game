using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(EnemyStat))]
[RequireComponent(typeof(EnemyDieDropDown))]
public class EnemyAI : AI {

    [System.NonSerialized]
    public EnemyDieDropDown enemyDieDropDown;

    [System.NonSerialized]
    public GameObject followTarget;

    public Transform bornTransform;

    //attack
    [System.NonSerialized]
    public bool isAttacking = false;
    [System.NonSerialized]
    public bool cancelAttack = true;
    public float attackDelay = 0.5f;
    public float attackSpeed = 1f;
    public float[] animationLenthRatioToInstanceBullet = new float[3] { 0.25f, 0.25f, 0.5f };
    public AnimationClip[] attackAnimationClips = new AnimationClip[3];
    public GameObject attackPrefab;
    public Transform attackPoint;
    //

    public float approachDistance = 2;
    public float detectRange = 15;
    public float lostSight = 20;
    [System.NonSerialized]
    public float distanceToTarget;

    [System.NonSerialized]
    public bool isFlinch = false;
    Vector3 flinchDirection = Vector3.back;

    public EnemyHealthBar healthBarPrefab;
    EnemyHealthBar enemyHealthBar = null;

    private void OnEnable()
    {
        if (enemyHealthBar != null)
            enemyHealthBar.gameObject.SetActive(true);
            //enemyHealthBar.SetupHealthBar(this.stat, player.transform);
    }

    private void Start()
    {
        aI_Type = AI_Type.Enemy;
        enemyDieDropDown = GetComponent<EnemyDieDropDown>();
        characterController = GetComponent<CharacterController>();
        stateMachine = GetComponent<StateMachine>();
        animator = GetComponent<Animator>();
        stat = GetComponent<EnemyStat>();
        stateMachine.SetUpMachine(gameObject.name, this);
        stateMachine.ChangeState("idle");

        if (healthBarPrefab != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            enemyHealthBar = Instantiate<EnemyHealthBar>(healthBarPrefab, GameplayManager.instance.screenUiController.healthBarParent);
            enemyHealthBar.SetupHealthBar(this.stat, player.transform);
        }
    }

    private void Update()
    {
        UseGravity();

        if (stat.isDead)
            return;

        if (isFlinch)
        {
            characterController.SimpleMove(flinchDirection * 6);
            CancelAttack();
            return;
        }

        if (stateMachine != null)
            stateMachine.UpdateState();

        characterController.SimpleMove(new Vector3(0, -9.81f, 0));

        if (followTarget != null)
        {
            distanceToTarget = (followTarget.transform.position - this.transform.position).magnitude;
        }
    }

    public void ReLive()
    {
        characterController.enabled = true; 
        transform.position = bornTransform.position;
        stat.RestEmenyStats();
        gameObject.SetActive(true);
        SwitchFirstState("idle");
    }

    public void FindNearestTarget()
    {
        float findingRadius = detectRange;

        Collider[] objectsAroundMe = Physics.OverlapSphere(transform.position, findingRadius);

        foreach (var go in objectsAroundMe)
        {
            if (go.CompareTag("Player"))
            {
                followTarget = go.gameObject;
                stateMachine.ChangeState("findingTarget");
                return;
            }
        }
    }

    public void TargetFleed()
    {
        followTarget = null;
        distanceToTarget = Mathf.Infinity;
    }

    public void CancelAttack()
    {
        cancelAttack = true;
    }

    public void Die()
    {
        stateMachine.ChangeState("deathState");
    }

    void Flinch(Vector3 dir)
    {
        flinchDirection = dir;

        if (followTarget != null)
            transform.LookAt(followTarget.transform);

        animator.Play("hurt");
        StartCoroutine(KnockBack());
    }

    IEnumerator KnockBack()
    {
        isFlinch = true;
        yield return new WaitForSeconds(0.2f);
        isFlinch = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lostSight);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, approachDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(moveStateProperties.center.position, moveStateProperties.maxDistanceToCenter);
    }
}
