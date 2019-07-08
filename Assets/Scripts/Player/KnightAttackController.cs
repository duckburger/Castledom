using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackController : MonoBehaviour
{
    [SerializeField] List<KnightAttack> availableAttacks = new List<KnightAttack>();
    [Space(10)]
    [SerializeField] AttackCollider attackCollier;
    Animator animator;
    AudioClip currentPreSwingAudioClip;
    AudioClip currentSwingAudioClip;
    int currentWeaponLayer = -1;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();
        if (Input.GetMouseButtonUp(0))
            ReleaseAttack();

    }

    void Attack()
    {
        // string attackAnimation = attackAnimationsNames[Random.Range(0, attackAnimationsNames.Count)];
        // animator?.Play(attackAnimation);
        animator?.SetBool("preSwing", true);
    }

    void ReleaseAttack()
    {
        animator?.SetBool("preSwing", false);
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
