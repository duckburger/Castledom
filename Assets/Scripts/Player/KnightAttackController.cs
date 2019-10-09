using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackController : MonoBehaviour
{
    [SerializeField] KnightAttack[] availableAttacks;
    [Space(10)]
    [SerializeField] PlayerAttackCollider attackCollider;
    [SerializeField] GameObject attackDirArrow;
    [SerializeField] Animator animator;
    [SerializeField] bool showAttackDirection = true;

    KnightMovement knightMovement;
    KnightRotation knightRotator;
    AudioClip currentPreSwingAudioClip;
    AudioClip currentSwingAudioClip;
    int currentWeaponLayer = -1;
    int currentAttackIndex = 0;

    public AudioClip CurrentPreSwingAudioClip => currentPreSwingAudioClip;
    public AudioClip CurrentSwingAudioClip => currentSwingAudioClip;

    private void Awake()
    {
        if (!animator)
            animator = GetComponent<Animator>();
        knightRotator = GetComponent<KnightRotation>();
        knightMovement = GetComponent<KnightMovement>();       
    }

    private void Start()
    {
        DeActivateAttackCollider();
    }

    public void SetupAttacksFromHandWeapon(WeaponStatFile weaponEquipped)
    {
        availableAttacks = weaponEquipped.availableAttacks;
        currentWeaponLayer = weaponEquipped.animationLayerIndex;       

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

        if (weaponEquipped.availableAttacks[0].attackSoundFx.Count > 0) // Take sounds from both attacks somehow
        {
            currentSwingAudioClip = weaponEquipped.availableAttacks[0].attackSoundFx[Random.Range(0, weaponEquipped.availableAttacks[0].attackSoundFx.Count - 1)];
        }
    }

    void Update()
    {
        // PRIMARY ATTACK
        if (Input.GetMouseButtonDown(0))
            PrimaryPreSwing();
        if (Input.GetMouseButtonUp(0))
            PrimarySwing();

        // SECONDARY ATTACK
        if (Input.GetMouseButtonDown(1))
            SecondaryPreSwing();
        if (Input.GetMouseButtonUp(1))
            SecondarySwing();

        // KICK (TERTIARY)
        if (Input.GetKeyDown(KeyCode.Q))
            PreKick();
        if (Input.GetKey(KeyCode.Q))
        {
            knightRotator?.RotateLegsTowardsMouse();
            knightMovement?.StopMovement(true);
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Kick();
            knightMovement?.StopMovement(false);
        }
    }

    void PrimaryPreSwing()
    {
        currentAttackIndex = availableAttacks[0].attackSelectorIndex;
        animator.SetInteger("attackSelector", currentAttackIndex);
        animator?.SetBool("preSwing", true);
        if (showAttackDirection && attackDirArrow)
            attackDirArrow.SetActive(true);
    }

    void PrimarySwing()
    {
        animator?.SetBool("preSwing", false);
        if (showAttackDirection && attackDirArrow)
            attackDirArrow.SetActive(false);
    }

    void SecondaryPreSwing()
    {
        if (availableAttacks.Length > 1)
        {
            currentAttackIndex = availableAttacks[1].attackSelectorIndex;
            animator.SetInteger("attackSelector", currentAttackIndex);
            animator?.SetBool("preSwing", true);
            if (showAttackDirection && attackDirArrow)
                attackDirArrow.SetActive(true);
        }        
    }

    void SecondarySwing()
    {
        animator?.SetBool("preSwing", false);
        if (showAttackDirection && attackDirArrow)
            attackDirArrow.SetActive(false);
    }

    void PreKick()
    {
        animator?.SetBool("preKick", true);
    }
    
    void Kick()
    {
        animator?.SetBool("preKick", false);
    }

    #region Activating / Deactivating

    public void ActivateAttackCollider()
    {
        attackCollider?.EnableAttackCollider();
    }

    public void DeActivateAttackCollider()
    {
        attackCollider?.DisableAttackCollider();
    }

    public void ActivateColliderKickMode()
    {
        attackCollider?.EnableKickCollider();
    }

    public void DeActivateColliderKickMode()
    {
        attackCollider?.DisableKickCollider();
    }


    #endregion

}
