using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {

    public Animator animator;
    public MovementController movement;
    public CharacterController controller;
    public PlayerStat playerStat;

    private bool jumping = false;
    private bool running = false;
    private bool walking = false;
    private bool isDead = false;

    private void Update()
    {
        if(playerStat.isDead && !isDead)
        {
            isDead = true;
            animator.SetBool("die", isDead);
            return;
        }
        if(isDead && !playerStat.isDead)
        {
            isDead = false;
            animator.SetBool("die", isDead);
            return;
        }

        if ((controller.collisionFlags & CollisionFlags.Below) != 0)
        {
            if (!movement.isControllable || GameplayManager.IsPauseInput)
                return;

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            animator.SetFloat("vertical", vertical);
            animator.SetFloat("horizontal", horizontal);

            animator.SetFloat("velocity", movement.movement.maxSpeed);

            //CheckRunning(vertical, horizontal);
            //CheckWalking(vertical, horizontal);

            if (jumping == true)
            {
                jumping = false;
                animator.SetBool("jumping", jumping);

            }
        }
        else
        {
            if (!movement.isControllable)
                return;

            jumping = true;
            animator.SetBool("jumping", jumping);
        }
    }
    /*
    void CheckRunning(float v, float h)
    {
        if (Input.GetKey(KeyCode.R) && (v != 0 ))
        {
            if (!running)
            {
                running = true;
                animator.SetBool("running", running);
            }
        }
        else
        {
            if (running)
            {
                running = false;
                animator.SetBool("running", running);
            }

        }
    }

    void CheckWalking(float v, float h)
    {
       if(v != 0 || h != 0)
        {
            if (!walking)
            {
                walking = true;
                animator.SetBool("walking", walking);
            }
        }
        else
        {
            if (walking)
            {
                walking = false;
                animator.SetBool("walking", walking);
            }
        }
    }
    */
    public void PlayAttackAnim(string Atkname, float attackSpeed)
    {
        //animator.SetBool("jumping", false);
        animator.SetFloat("attackSpeed", attackSpeed);
        animator.Play(Atkname);
    }

    public void PlayAnim(string aniName)
    {
        animator.Play(aniName);
    }
}
