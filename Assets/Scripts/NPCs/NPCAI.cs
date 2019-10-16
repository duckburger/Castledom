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
    [SerializeField] Vector2? targetLastKnownPos = null;
    [SerializeField] bool alertNearbyIfAttacked = false;
    [Space(10)]
    [SerializeField] float stamina = 100f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRecoverySpeed = 0.5f;
    [SerializeField] float sprintStaminaDepletion = 0.5f;
    [SerializeField] bool isRunning = false;

    NPCVision visionController;
    NPCAttackCollider npcAttackCollider;
    NPCStatusIcon statusIcon;
    NPCRotator npcRotator;
    PolyNavAgent polynavAgent;
    Transform currentTarget;
    Health healthController;
    CharHitDetector hitDetector;
    NPCAnimator npcAnimator;
    NPCNearbyEventBroadcaster eventBroadcaster;

    Vector2? preCombatPosition = null;

    [Task] bool inCombat = false;
    [Task] bool recoveringStamina = false;
    [Task] bool isStunned = false;

    public bool RecoveringStamina => recoveringStamina;
    public bool IsRunning => isRunning;
    public float Stamina => stamina;
    public bool IsStunned
    {
        get => isStunned;
        set => isStunned = value;
    }

    public bool InCombat => inCombat;

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
        eventBroadcaster = GetComponentInChildren<NPCNearbyEventBroadcaster>();
        visionController = GetComponentInChildren<NPCVision>();

        healthController.onHealthDecreased += ReactToAgression;
    }

    private void Update()
    {
        if (isRunning && !recoveringStamina)
        {
            SpeedUp();
            stamina -= Time.deltaTime * sprintStaminaDepletion;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);            
        }

        if (stamina <= 0)
        {
            SlowDown();
            recoveringStamina = true;
        }

        if (recoveringStamina)
        {
            SlowDown();
            stamina += Time.deltaTime * staminaRecoverySpeed;
            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            if (Mathf.Approximately(stamina, maxStamina))
                recoveringStamina = false;
            else
                recoveringStamina = true;
        }        
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
        if (polynavAgent.hasPath && (Vector2)transform.position != polynavAgent.primeGoal)
        {
            return;
        }        
        else
        {
            Task.current.Succeed();
        }
    }

    #endregion

    #region Combat

    public void SetInCombatExternally(Health attacker, bool indirect = false)
    {
        if (!inCombat)
        {
            inCombat = true;
            combatTarget = attacker;
            preCombatPosition = transform.position;
            if (eventBroadcaster && alertNearbyIfAttacked && indirect)
                eventBroadcaster.BroadcastAttackReaction(combatTarget);
            visionController?.ShowVisionCone(false);
        }
    }

    public void ReactToAgression()
    {
        if (!inCombat && ((1 << hitDetector.LastHitBy.gameObject.layer) | hostileChars) == hostileChars)
        {
            inCombat = true;
            combatTarget = hitDetector.LastHitBy.GetComponentInParent<Health>();
            preCombatPosition = transform.position;
            if (eventBroadcaster && alertNearbyIfAttacked)
                eventBroadcaster.BroadcastAttackReaction(combatTarget);
            visionController?.ImmediatelyAlertExternal();
            visionController?.ShowVisionCone(false);
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
    void ChooseCombatTargetAsDestination()
    {
        if (combatTarget)
        {
            AssignDestination(combatTarget.transform);
        }
        else
        {
            Task.current.Fail();
        }
        
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

    #region Search and Combat Transitions

    [Task]
    void ExitCombat()
    {
        inCombat = false;
        combatTarget = null;
        visionController?.ShowVisionCone(true);
        Task.current.Succeed();
    }

    [Task]
    void LoseLastKnownPosition()
    {
        targetLastKnownPos = null;
        Task.current.Succeed();
    }

    public void LoseSightOfCombatTarget()
    {
        if (combatTarget)
        {        
            targetLastKnownPos = combatTarget.transform.position;
            ExitCombat();
            visionController.Searching = true;
        }
    }

    [Task]
    bool HasLastKnownPosition()
    {
        return targetLastKnownPos != null;
    }

    [Task]
    bool HasReachedLastKnown()
    {
        if (targetLastKnownPos != null)
            return transform.position == targetLastKnownPos;
        else
            return false;
    }

    [Task]
    void SetLastKnownAsTarget()
    {
        if (targetLastKnownPos != null)
        {
            polynavAgent?.SetDestination((Vector2)targetLastKnownPos);
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    void ChooseRandomNearLastKnown()
    {
        if (targetLastKnownPos != null)
        {
            Vector2? newTarget = targetLastKnownPos += new Vector2?(new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)));
            polynavAgent?.SetDestination((Vector2)newTarget);
            Task.current.Succeed();
        }
    }

    [Task]
    void SetPreCombatPositionAsTarget()
    {
        if (preCombatPosition != null)
        {
            polynavAgent?.SetDestination((Vector2)preCombatPosition);
            visionController.Searching = false;
            Task.current.Succeed();
        }
    }

    #endregion

    #region Stamina

    [Task]
    bool EnoughStaminaToRun()
    {
        return stamina > 0f;
    }

    [Task]
    void TurnOnRun()
    {
        isRunning = true;
        while (!Mathf.Approximately(polynavAgent.maxSpeed, runSpeed))
            polynavAgent.maxSpeed = Mathf.Lerp(polynavAgent.maxSpeed, runSpeed, Time.deltaTime * 25f);
        Task.current.Succeed();
    }

    [Task]
    void TurnOffRun()
    {
        isRunning = false;
        polynavAgent.maxSpeed = walkSpeed;
        Task.current.Succeed();
    }

    void SlowDown()
    {
        polynavAgent.maxSpeed = walkSpeed;
    }

    void SpeedUp()
    {
        polynavAgent.maxSpeed = Mathf.Lerp(polynavAgent.maxSpeed, runSpeed, Time.deltaTime * 25f);
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
        visionController?.ImmediatelyAlertExternal();
    }

    #endregion

}
