using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttackCollider : AttackCollider
{
    [SerializeField] KnightInventory playerInventory;
    [SerializeField] float minTimeBetweenAttacks = 0.15f;

    bool canRegisterAttack = true;
    KnightSounds knightSounds;


    private void Start()
    {
        circleCollider = GetComponent<Collider2D>();
        knightSounds = GetComponentInParent<KnightSounds>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);   
    }

    public override void HandleCollision(Collider2D collider)
    {
        base.HandleCollision(collider);

        if (!playerInventory)
        {
            Debug.LogError("Connect player inventory to the player's attack collider");
            return;
        }

        if (!canRegisterAttack)
            return;

        Health hitEnemyHealth = collider.GetComponent<Health>();
        if (hitEnemyHealth != null) //TODO: Add friend/foe for check
        {
            int length = playerInventory.EquippedWeapon.availableAttacks[0].hitSounds.Length;
            if (length > 0)
            {
                int index = UnityEngine.Random.Range(0, length);
                knightSounds.PlaySound(playerInventory.EquippedWeapon.availableAttacks[0].hitSounds[index]);
            }
            float dmg = hitEnemyHealth.armored ? playerInventory.EquippedWeapon.armoredDmg : playerInventory.EquippedWeapon.baseDmg;
            Transform body = collider.GetComponent<NPCRotator>().body;
            float dot = Vector2.Dot(transform.parent.up, body.up);
            if (dot > 0.2f)
            {
                // Player is behind the target, add extra damage
                dmg *= 2f;
            }
            NPCHitDetector hitDetector = collider.GetComponent<NPCHitDetector>();
            if (hitDetector)
            {
                hitDetector.LastHitBy = transform;
            }

            hitEnemyHealth.AdjustHealth(-dmg);
           
            canRegisterAttack = false;
            StartCoroutine(TimerForAttackRegistration());
        }
    }


    IEnumerator TimerForAttackRegistration()
    {
        yield return new WaitForSeconds(minTimeBetweenAttacks);
        canRegisterAttack = true;
    }
}
