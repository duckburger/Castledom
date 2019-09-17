using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackController : MonoBehaviour
{
    [SerializeField] List<KnightAttack> availableAttacks = new List<KnightAttack>();
    [Space(10)]
    [SerializeField] AttackCollider attackCollier;
    [SerializeField] GameObject attackDirArrow;
    [SerializeField] bool showAttackDirection = true;
    Animator animator;
    AudioClip currentPreSwingAudioClip;
    AudioClip currentSwingAudioClip;
    int currentWeaponLayer = -1;
    int currentAttackIndex = 0;

    public AudioClip CurrentPreSwingAudioClip => currentPreSwingAudioClip;
    public AudioClip CurrentSwingAudioClip => currentSwingAudioClip;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetupAttack();
    }

    public void SetupAttack()
    {
        if (availableAttacks.Count > 0)
        {
            currentWeaponLayer = availableAttacks[0].animationLayerIndex;
        }

        if (currentWeaponLayer >= 0)
        {
            animator.SetLayerWeight(currentWeaponLayer, 1);
        }

        for (int i = 0; i < animator.layerCount; i++)
        {
            if (i != currentWeaponLayer && i > 1)
            {
                animator.SetLayerWeight(i, 0);
            }
        }

        if (availableAttacks[0].attackSoundFx.Count > 0)
        {
            currentSwingAudioClip = availableAttacks[0].attackSoundFx[Random.Range(0,  availableAttacks[0].attackSoundFx.Count - 1)];
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
            PreSwing();
        if (Input.GetMouseButtonUp(0))
            Swing();
    }

    void PreSwing()
    {
        currentAttackIndex = availableAttacks[0].attackIndices[Random.Range(0, availableAttacks[0].attackIndices.Length)];
        animator.SetInteger("attackSelector", currentAttackIndex);
        animator?.SetBool("preSwing", true);
        if (showAttackDirection && attackDirArrow)
            attackDirArrow.SetActive(true);
    }

    void Swing()
    {
        animator?.SetBool("preSwing", false);
        if (showAttackDirection && attackDirArrow)
            attackDirArrow.SetActive(false);
    }

    public void ActivateAttackCollider()
    {
        attackCollier.isOn = true;
    }

    public void DeActivateAttackCollider()
    {
        attackCollier.isOn = false;
    }

}
