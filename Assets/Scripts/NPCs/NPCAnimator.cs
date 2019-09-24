using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NPCAnimator : MonoBehaviour
{
    Animator animator;
    PolyNavAgent navAgent;
    NPCAttackCollider npcAttackCollider;
    NPCRotator npcRotator;
    ParticleSystem sprintParticles;
    NPCAI npcAI;

    bool isOn = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<PolyNavAgent>();
        npcAttackCollider = GetComponentInChildren<NPCAttackCollider>(true);
        npcRotator = GetComponent<NPCRotator>();
        if (npcRotator)
        {
            sprintParticles = npcRotator.body.GetComponent<ParticleSystem>();
        }
        npcAI = GetComponent<NPCAI>();
    }

    private void Update()
    {
        if (!isOn)
            return;

        if (navAgent.hasPath)
        {
            animator.SetBool("isWalking", true);
            if (npcAI.IsRunning)
                sprintParticles?.Play();
            else
                sprintParticles?.Clear();
        }
        else
        {
            animator.SetBool("isWalking", false);
            sprintParticles?.Pause();
            sprintParticles?.Clear();                   
        }
    }

    public void SetIdle(bool enable)
    {
        if (enable)
        {
            isOn = false;
            animator?.SetBool("isAttacking", false);
            animator?.SetBool("isWalking", false);
            for (int i = 1; i < animator.layerCount; i++)
            {
                animator?.SetLayerWeight(i, 0);
            }
        }
        else
        {
            isOn = true;
            for (int i = 1; i < animator.layerCount; i++)
            {
                animator?.SetLayerWeight(i, 1);
            }
        }
        
    }

    public void ActivateAttack()
    {
        animator?.SetBool("isAttacking", true);
    }

    public void DeactivateAttack()
    {
        animator?.SetBool("isAttacking", false);
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
