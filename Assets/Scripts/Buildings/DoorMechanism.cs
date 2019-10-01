using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HingeJoint2D))]
public class DoorMechanism : MonoBehaviour, IInteractiveObject, IBreakable
{
    [SerializeField] Vector2 lockedLimitVals = Vector2.zero;
    [SerializeField] Vector2 unlockedLimitVals = new Vector2(-90f, 90f);
    [Space]
    [SerializeField] bool unlockableByPlayer = true;
    [SerializeField] bool breakable = true;
    [SerializeField] bool isUnlocked = true;
    [Space]
    [SerializeField] GameObject brokenVersion;
    [SerializeField] GameObject navmeshObstacle;
    HingeJoint2D doorJoint;
    JointAngleLimits2D lockedLimits = new JointAngleLimits2D();
    JointAngleLimits2D unlockedLimits = new JointAngleLimits2D();
    Rigidbody2D rb;
    Rigidbody2D[] brokenPiecesRBs;

    event Action<bool> onLockStatusChanged;

    public bool IsUnlocked
    {
        get => isUnlocked;
        set 
        {
            isUnlocked = value;
            onLockStatusChanged?.Invoke(value);
        }
    }

    public bool UnlockableByPlayer => unlockableByPlayer;

    private void OnValidate()
    {
        IsUnlocked = isUnlocked;
    }

    private void Start()
    {
        onLockStatusChanged += LockStatusChanged;
        doorJoint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();

        lockedLimits.min = lockedLimitVals.x;
        lockedLimits.max = lockedLimitVals.y;
        unlockedLimits.min = unlockedLimitVals.x;
        unlockedLimits.max = unlockedLimitVals.y;
    }

    public void Toggle()
    {
        if (isUnlocked)
            LockDoor();
        else
            UnlockDoor();
    }

    void LockStatusChanged(bool isUnlocked)
    {
        if (isUnlocked)
            UnlockDoor();
        else
            LockDoor();
    }

    public void UnlockDoor()
    {
        doorJoint.limits = unlockedLimits;
        isUnlocked = true;
        if (navmeshObstacle)
            navmeshObstacle.gameObject.SetActive(false);
    }

    public void LockDoor()
    {
        doorJoint.limits = lockedLimits;
        isUnlocked = false;
        if (navmeshObstacle)
            navmeshObstacle.gameObject.SetActive(true);
    }

    public void Interact()
    {
        Toggle();
    }

    public bool Interactable()
    {
        return unlockableByPlayer;
    }

    public bool Breakable()
    {
        return breakable;
    }

    public void Break()
    {
        // Break joint
        doorJoint.enabled = false;
        gameObject.layer = 16;
        // Spawn pieces
        //if (brokenVersion)
        //{
        //    GameObject spawnedBroken = Instantiate(brokenVersion, transform.position, Quaternion.identity, transform.parent);
        //    brokenPiecesRBs = spawnedBroken.GetComponentsInChildren<Rigidbody2D>();
        //}

        // Make pieces fly
    }

    public GameObject BrokenVersion()
    {
        return brokenVersion;
    }

    public void BreakWithForce(Vector2 incomingForce)
    {
        Break();
        rb.AddForce(incomingForce, ForceMode2D.Force);        
        //Destroy(gameObject);
    }
}
