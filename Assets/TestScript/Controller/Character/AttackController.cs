using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AnimController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(CharacterStat))]

public class AttackController : MonoBehaviour {

    public AnimationClip[] attacksClips;
    public AnimationClip hurtClip;
    public GameObject attackPrefab;
    public Transform attackTransform;

    public float moveDistance = 3;
    public float delayAtferAttackChains = 1.5f;
    [Range(1, 2.5f)]
    public float attackSpeed = 1;

    int c = 0;
    int combo = 0;
    float nextFire = 0;
    bool isCasting = false;
    bool isMeleeForword = false;
    bool isFlinch = false;

    CharacterController controller;
    AnimController animController;
    MovementController moveController;
    PlayerStat playerStat;

    private void Start()
    {
        animController = GetComponent<AnimController>();
        moveController = GetComponent<MovementController>();
        controller = GetComponent<CharacterController>();
        playerStat = GetComponent<PlayerStat>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Flinch(Vector3.zero);
        }

        if (GameplayManager.IsPauseInput || playerStat.isDead)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("asdasd");
            return;
        }

        if (isFlinch)
        {
            controller.SimpleMove(transform.TransformDirection(Vector3.back) * 6);
            return;
        }

        if (isMeleeForword)
        {
            Vector3 v3 = transform.TransformDirection(Vector3.forward);
            moveController.movement.calculatedVelocity = new Vector3(0, moveController.movement.calculatedVelocity.y, 0);
            controller.SimpleMove(v3 * moveDistance);
        }

        if (Input.GetMouseButton(0))
        {

            if (Time.time > (nextFire + 0f))
            {
                c = 0;
            }
            if (attacksClips.Length >= 1 && !isCasting)
            {
                combo += 1;
                StartCoroutine(AttackCombo());
            }
        }
    }

   
    IEnumerator AttackCombo()
    {
        if (c >= attacksClips.Length)
        {
            c = 0;
        }

        if (attacksClips[c])
        {
            isCasting = true;
            moveController.isControllable = false;

            while(combo > 0)
            {
                animController.PlayAttackAnim(attacksClips[c].name, attackSpeed);

                StartCoroutine(MeleeAttackForward());
                StartCoroutine(GenerateAtkBullet(attacksClips[c].length * 1 / 4));

                nextFire = Time.time + attacksClips[c].length / attackSpeed;
                c++;
                combo -= 1;

                if (c >= attacksClips.Length)
                {
                    c = 0;
                    //atkDelay = true;
                    yield return new WaitForSeconds(delayAtferAttackChains);
                    //atkDelay = false;
                }
                else
                {
                    float f = attacksClips[c - 1].length * .75f / attackSpeed;
                    yield return new WaitForSeconds(f);

                }
            }
            isCasting = false;
            moveController.isControllable = true;
        }
    }

    IEnumerator MeleeAttackForward()
    {
        isMeleeForword = true;
        yield return new WaitForSeconds(0.2f);
        isMeleeForword = false;
    }

    IEnumerator GenerateAtkBullet(float time)
    {
        yield return new WaitForSeconds(time);
        if (!isFlinch)
        {
            GameObject atkBullet = Instantiate(attackPrefab, attackTransform.position, attackTransform.rotation) as GameObject;
            atkBullet.GetComponent<AttackBullet>().SetupBullet(gameObject, playerStat.attack.GetAdjsutedValue(), playerStat.spAttack.GetAdjsutedValue());
        }
    }

    void Flinch(Vector3 dir)
    {
        if (playerStat.isDead)
            return;

        animController.PlayAnim(hurtClip.name);
        StartCoroutine(KnockBack());
    }

    IEnumerator KnockBack()
    {
        isFlinch = true;
        moveController.isControllable = false;

        yield return new WaitForSeconds(0.2f);

        isFlinch = false;
        moveController.isControllable = true;

    }
}
