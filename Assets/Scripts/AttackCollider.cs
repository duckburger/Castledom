using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool isOn = false;
    public bool kickMode = false;

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

    public virtual void EnableKickCollider()
    {
        isOn = true;
        kickMode = true;
        circleCollider.enabled = true;
    }

    public virtual void DisableKickCollider()
    {
        isOn = false;
        kickMode = false;
        circleCollider.enabled = false;
    }
 
    public virtual void HandleRegularAttackCollision(Collider2D collider)
    {
        if (!isOn)
            return;
    }

    public virtual void HandleKickAttackCollision(Collider2D collider)
    {
        if (!isOn)
            return;
    }
}
