using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using Panda;

public class NPCAI : MonoBehaviour
{
    [SerializeField] LayerMask hostileChars;
    [SerializeField] float combatRange = 3f;
    [SerializeField] float runSpeed = 2.5f;
    [SerializeField] float walkSpeed = 1.8f;
    [SerializeField] Health combatTarget = null;
    [Space]
    [SerializeField] float stamina = 100f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRecoverySpeed = 0.5f;
    [SerializeField] float sprintStaminaDepletion = 0.5f;
    [SerializeField] bool isRunning = false;

    NPCAttackCollider npcAttackCollider;
    NPCStatusIcon statusIcon;
    NPCRotator npcRotator;
    PolyNavAgent polynavAgent;
    Transform currentTarget;
    Health healthController;
    CharHitDetector hitDetector;
    NPCAnimator npcAnimator;

    [Task] bool inCombat = false;
    [Task] bool recoveringStamina = false;
    [Task] bool isStunned = false;

    public bool IsRunning => isRunning;
    public bool IsStunned
    {
        get => isStunned;
        set => isStunned = value;
    }

    Coroutine stunTimer = null;

    public Transform CombatTarget
    {
        get
        {
            if (combatTarget)
                return combatTarget.transform;
            else
                return null;
        }
    }
      

    private void Awake()
    {
        polynavAgent = GetComponent<PolyNavAgent>();
        healthController = GetComponent<Health>();
        hitDetector = GetComponent<CharHitDetector>();
        npcAnimator = GetComponent<NPCAnimator>();
        npcRotator = GetComponent<NPCRotator>();
        statusIcon = GetComponentInChildren<NPCStatusIcon>();
        npcAttackCollider = GetComponentInChildren<NPCAttackCollider>();

        healthController.onHealthDecreased += ReactToAgression;
    }


    #region Walking

    public void AssignDestination(Transform target)
    {
        currentTarget = target;
        polynavAgent.SetDestination(currentTarget.position);
    }

    [Task]
    void FindEndOfRoad()
    {
        RoadNPCSpawner roadSpawner = FindObjectOfType<RoadNPCSpawner>();
        if (!roadSpawner)
        {
            Debug.LogError("No road spawner found in the scene");
            Task.current.Fail();
        }

    }

    [Task]
    void GoToDestination()
    {
        //if (polynavAgent.pathPending)
        if (polynavAgent.pathPending)
            Debug.Log($"{Task.current.debugInfo}");
        else
            Task.current.Succeed();
    }

    #endregion

    #region Agressive Behaviour

    public void ReactToAgression()
    {
        if (!inCombat && ((1 << hitDetector.LastHitBy.gameObject.layer) | hostileChars) == hostileChars)
        {
            inCombat = true;
            combatTarget = hitDetector.LastHitBy.GetComponentInParent<Health>();
        }
    }

    [Task]
    bool CombatTargetAlive()
    {
        if (combatTarget)
        {
            if (combatTarget.CurrentHealth <= 0)
            {
                combatTarget = null;
                inCombat = false;
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    [Task]
    bool WithinCombatRange()
    {
        if (combatTarget == null || combatTarget.enabled == false || !combatTarget.gameObject.activeSelf)
            return false;
        return Vector2.Distance(transform.position, combatTarget.transform.position) <= combatRange ? true : false;
    }

    [Task]
    void ChaseCombatTarget() // TODO: Add speed change + Add stamina system??
    {
        npcAnimator.DeactivateAttack();
        if (combatTarget == null || combatTarget.enabled == false || !combatTarget.gameObject.activeSelf)
        {
            Task.current.Complete(false);
            return;
        }

        if (polynavAgent.primeGoal != (Vector2)combatTarget.transform.position)
        polynavAgent.SetDestination(combatTarget.transform.position);

        if (polynavAgent.pathPending)
            Debug.Log($"{Task.current.debugInfo}");
        else
            Task.current.Succeed();
    }

    [Task]
    void AttackTarget()
    {
        npcAnimator.ActivateAttack();
        if (!CombatTargetAlive())
            combatTarget = null;
            
        Task.current.Succeed();
    }

    #endregion

    #region Stamina

    [Task]
    bool EnoughStaminaToRun()
    {
        return stamina > 0f;
    }

    [Task]
    void RestoreStamina()
    {
        stamina += Time.deltaTime * staminaRecoverySpeed;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);        
        if (stamina == maxStamina)
            recoveringStamina = false;
        else
            recoveringStamina = true;
        Task.current.Succeed();
    }

    [Task]
    void DepleteStamina()
    {
        if (isRunning)
        {
            stamina -= Time.deltaTime * sprintStaminaDepletion;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            if (stamina <= 0)
            {
                recoveringStamina = true;
            }
            Task.current.Succeed();
        }       
    }

    [Task]
    void SpeedUpToRun()
    {
        isRunning = true;
        polynavAgent.maxSpeed = Mathf.Lerp(polynavAgent.maxSpeed, runSpeed, Time.deltaTime * 25f);
        Task.current.Succeed();
    }

    [Task]
    void SlowDownToWalk()
    {
        isRunning = false;
        polynavAgent.maxSpeed = walkSpeed;
        Task.current.Succeed();
    }

    #endregion

    #region Stun

    public void Stun(bool enable, float duration)
    {
        if (duration <= 0)
            return;

        if (stunTimer != null)
            StopCoroutine(stunTimer);

        if (enable)
        {            
            stunTimer = StartCoroutine(StunTimer(duration));
        }
        else
        {
            isStunned = false;
        }
    }


    IEnumerator StunTimer(float stunDuration)
    {
        isStunned = true;
        npcRotator?.EnableRotator(false);
        statusIcon?.ShowStunnedStatus();
        npcAnimator?.SetIdle(true);
        npcAttackCollider?.DisableAttackCollider();
        polynavAgent.enabled = false;
        yield return new WaitForSeconds(stunDuration);
        polynavAgent.enabled = true;
        npcAnimator?.SetIdle(false);
        statusIcon?.Disable();
        npcRotator?.EnableRotator(true);
        isStunned = false;
    }

    #endregion

}
