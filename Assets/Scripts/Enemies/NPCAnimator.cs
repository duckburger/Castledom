using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

public class NPCAnimator : MonoBehaviour
{
    Animator animator;
    PolyNavAgent navAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<PolyNavAgent>();
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

}
