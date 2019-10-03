using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

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
    BoxCollider2D doorCollider;
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

    private void Awake()
    {
        onLockStatusChanged += LockStatusChanged;
        doorJoint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        doorCollider = GetComponent<BoxCollider2D>();

        lockedLimits.min = lockedLimitVals.x;
        lockedLimits.max = lockedLimitVals.y;
        unlockedLimits.min = unlockedLimitVals.x;
        unlockedLimits.max = unlockedLimitVals.y;
    }

    public async void Toggle()
    {
        if (isUnlocked)
            await LockDoor();
        else
            await UnlockDoor();
    }

    async void LockStatusChanged(bool isUnlocked)
    {
        if (isUnlocked)
            await UnlockDoor();
        else
            await LockDoor();
    }

    public async Task UnlockDoor()
    {
        doorCollider.enabled = false;
        doorJoint.limits = unlockedLimits;
        isUnlocked = true;
        if (navmeshObstacle)
            navmeshObstacle.gameObject.SetActive(false);
        await WaitForOpen();
    }

    async Task WaitForOpen()
    {
        while (Mathf.Abs(doorJoint.jointAngle) < 80)
        {
            await Task.Delay(2);
        }
        doorCollider.enabled = true;
    }

    public async Task LockDoor()
    {
        doorCollider.enabled = false;
        doorJoint.limits = lockedLimits;
        isUnlocked = false;
        if (navmeshObstacle)
            navmeshObstacle.gameObject.SetActive(true);
        await WaitForClose();
    }

    async Task WaitForClose()
    {
        while (Mathf.Abs(doorJoint.jointAngle) > 10)
        {
            await Task.Delay(2);           
        }
        doorCollider.enabled = true;
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
        // Spawn particles
 

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
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }
}
