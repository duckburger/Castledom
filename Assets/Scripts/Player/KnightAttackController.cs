using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAttackController : MonoBehaviour
{
    [SerializeField] KnightAttack availableAttack;
    [Space(10)]
    [SerializeField] PlayerAttackCollider attackCollider;
    [SerializeField] GameObject attackDirArrow;
    [SerializeField] bool showAttackDirection = true;

    KnightMovement knightMovement;
    KnightRotation knightRotator;
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
        knightRotator = GetComponent<KnightRotation>();
        knightMovement = GetComponent<KnightMovement>();
        DeActivateAttackCollider();
    }

    public void SetupNewAttack(KnightAttack newAttack)
    {
        availableAttack = newAttack;
        currentWeaponLayer = availableAttack.animationLayerIndex;       

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

        if (availableAttack.attackSoundFx.Count > 0)
        {
            currentSwingAudioClip = availableAttack.attackSoundFx[Random.Range(0, newAttack.attackSoundFx.Count - 1)];
        }
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
            PreSwing();
        if (Input.GetMouseButtonUp(0))
            Swing();
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

    void PreSwing()
    {
        currentAttackIndex = availableAttack.attackIndices[Random.Range(0, availableAttack.attackIndices.Length)];
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
