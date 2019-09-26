using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class DoorMechanism : MonoBehaviour, IInteractiveObject
{
    [SerializeField] Vector2 lockedLimitVals = Vector2.zero;
    [SerializeField] Vector2 unlockedLimitVals = new Vector2(-90f, 90f);

    HingeJoint2D doorJoint;
    JointAngleLimits2D lockedLimits = new JointAngleLimits2D();
    JointAngleLimits2D unlockedLimits = new JointAngleLimits2D();
    bool isUnlocked = true;

    public bool IsUnlocked => isUnlocked;

    private void Start()
    {
        doorJoint = GetComponent<HingeJoint2D>();

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

    public void UnlockDoor()
    {
        doorJoint.limits = unlockedLimits;
        isUnlocked = true;
    }

    public void LockDoor()
    {
        doorJoint.limits = lockedLimits;
        isUnlocked = false;
    }

    public void Interact()
    {
        Toggle();
    }
}
