using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State {


    public override void PreUpdate(AI ai)
    {
        ai.animator.SetBool("run", false);
    }

    public override void Update(AI ai)
    {
        ai.StartCoroutine(Attack(ai));
    }

    public override void Exit(AI ai)
    {

    }

    public IEnumerator Attack(AI ai)
    {
        EnemyAI enemyAI = (EnemyAI)ai;
        
        if (!enemyAI.isFlinch && !enemyAI.isAttacking)// and something like freeze
        {
            enemyAI.cancelAttack = false;
            enemyAI.isAttacking = true;
            //animation
            enemyAI.animator.SetFloat("attackSpeed", enemyAI.attackSpeed);
            int rng = UnityEngine.Random.Range(0, enemyAI.attackAnimationClips.Length);
            enemyAI.animator.Play(enemyAI.attackAnimationClips[rng].name);
            if (enemyAI.followTarget != null)
            {
                enemyAI.transform.LookAt(enemyAI.followTarget.transform);
            }

            yield return new WaitForSeconds(enemyAI.attackAnimationClips[rng].length * enemyAI.animationLenthRatioToInstanceBullet[rng] * 1/enemyAI.attackSpeed);

            if (!enemyAI.cancelAttack)
            {
                //instance attack prefab
                GameObject bullet = GameObject.Instantiate(enemyAI.attackPrefab, enemyAI.attackPoint.position, enemyAI.attackPoint.rotation) as GameObject;
                bullet.GetComponent<AttackBullet>().SetupBullet(enemyAI.gameObject, enemyAI.stat.attack.GetValue(), enemyAI.stat.spAttack.GetValue());
                yield return new WaitForSeconds(enemyAI.attackDelay);
                //check distance
                CheckDistance(enemyAI);

                enemyAI.isAttacking = false;
            }
            else
            {
                enemyAI.isAttacking = false;
            }
        }
    }

    void CheckDistance(AI ai)
    {
        EnemyAI enemyAI = (EnemyAI)ai;

        if (enemyAI.stat.isDead)
            return;

        if (enemyAI.distanceToTarget <= enemyAI.approachDistance)
            return;
        //Debug.Log("canceled!");
        enemyAI.CancelAttack();

        if (enemyAI.followTarget == null)
        {
            //ai.CancelAttack();
            enemyAI.stateMachine.ChangeState("move");
        }

        if (enemyAI.distanceToTarget > enemyAI.approachDistance && enemyAI.distanceToTarget < enemyAI.lostSight)
        {
            enemyAI.stateMachine.ChangeState("findingTarget");
        }
        else if(enemyAI.distanceToTarget > enemyAI.lostSight)
        {
            enemyAI.TargetFleed();
           // ai.CancelAttack();
            enemyAI.stateMachine.ChangeState("move");
        }

    }
}
