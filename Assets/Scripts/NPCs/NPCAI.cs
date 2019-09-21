using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using Panda;

public class NPCAI : MonoBehaviour
{
    [SerializeField] float combatRange = 3f;

    PolyNavAgent polynavAgent;
    Transform currentTarget;
    Health healthController;
    NPCHitDetector hitDetector;
    NPCAnimator npcAnimator;


    [Task]
    bool inCombat = false;
    Health combatTarget = null;

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
        hitDetector = GetComponent<NPCHitDetector>();
        npcAnimator = GetComponent<NPCAnimator>();

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
        if (polynavAgent.pathPending)
            Debug.Log($"{Task.current.debugInfo}");
        else
            Task.current.Succeed();
    }

    #endregion

    #region Agressive Behaviour

    public void ReactToAgression()
    {
        if (!inCombat && hitDetector.LastHitBy.gameObject.layer == 10)
        {
            inCombat = true;
            combatTarget = hitDetector.LastHitBy.GetComponentInParent<Health>();
        }
    }

    [Task]
    bool WithinCombatRange()
    {
        return Vector2.Distance(transform.position, combatTarget.transform.position) <= combatRange ? true : false;
    }

    [Task]
    void ChaseCombatTarget() // TODO: Add speed change + Add stamina system??
    {
        npcAnimator.DeactivateAttack();
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
    }

    #endregion

}
