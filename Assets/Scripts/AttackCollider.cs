using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool isOn = false;

    protected Collider2D circleCollider;

    public virtual void EnableAttackCollider()
    {
        isOn = true;
        circleCollider.enabled = true;
    }
    public virtual void DisableAttackCollider()
    {
        isOn = false;
        circleCollider.enabled = false;
    }
    public virtual void HandleCollision(Collider2D collider)
    {
        if (!isOn)
            return;
    }
}
