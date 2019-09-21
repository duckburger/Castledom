using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NPCAnimator : MonoBehaviour
{
    Animator animator;
    PolyNavAgent navAgent;
    NPCAttackCollider npcAttackCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<PolyNavAgent>();
        npcAttackCollider = GetComponentInChildren<NPCAttackCollider>(true);
    }

    private void Update()
    {
        if (navAgent.hasPath)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void ActivateAttack()
    {
        animator.SetBool("isAttacking", true);
    }

    public void DeactivateAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    public void ActivateAttackCollider()
    {
        npcAttackCollider?.EnableAttackCollider();
    }

    public void DeActivateAttackCollider()
    {
        npcAttackCollider?.DisableAttackCollider();
    }

}
