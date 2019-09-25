using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class DoorMechanism : MonoBehaviour
{
    [SerializeField] Vector2 lockedLimitVals = Vector2.zero;
    [SerializeField] Vector2 unlockedLimitVals = new Vector2(-90f, 90f);

    HingeJoint2D doorJoint;
    JointAngleLimits2D lockedLimits = new JointAngleLimits2D();
    JointAngleLimits2D unlockedLimits = new JointAngleLimits2D();


    private void Start()
    {
        doorJoint = GetComponent<HingeJoint2D>();

        lockedLimits.min = lockedLimitVals.x;
        lockedLimits.max = lockedLimitVals.y;
        unlockedLimits.min = unlockedLimitVals.x;
        unlockedLimits.max = unlockedLimitVals.y;
    }

    public void UnlockDoor()
    {
        doorJoint.limits = unlockedLimits;
    }

    public void LockDoor()
    {
        doorJoint.limits = lockedLimits;
    }


}
