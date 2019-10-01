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


    private void Awake()
    {
        circleCollider = GetComponent<Collider2D>();
        knightSounds = GetComponentInParent<KnightSounds>();
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!kickMode)
        {
            HandleRegularAttackCollision(other);
        }
        else
        {
            HandleKickAttackCollision(other);
            RegisterBreakableKick(other);
        }

    }

    public override void HandleRegularAttackCollision(Collider2D collider)
    {
        base.HandleRegularAttackCollision(collider);

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
            PlayHitSound(playerInventory.EquippedWeapon);
            float dmg = CalculateDamageAmount(collider, hitEnemyHealth, playerInventory.EquippedWeapon);
            ProcessHitDetector(collider);
            //PushCharacterBack(collider, playerInventory.EquippedWeapon);
            hitEnemyHealth.TryStun(playerInventory.EquippedWeapon);
            hitEnemyHealth.AdjustHealth(-dmg);
           
            canRegisterAttack = false;
            StartCoroutine(TimerForAttackRegistration());
        }
    }

    public override void HandleKickAttackCollision(Collider2D collider)
    {
        base.HandleKickAttackCollision(collider);

        if (!playerInventory)
        {
            Debug.LogError("Connect player inventory to the player's attack collider");
            return;
        }

        if (!canRegisterAttack)
            return;

        Health hitEnemyHealth = collider.GetComponent<Health>();
        if (hitEnemyHealth != null) //TODO: Add friend/foe check
        {
            PlayHitSound(playerInventory.KickWeapon);
            float dmg = CalculateDamageAmount(collider, hitEnemyHealth, playerInventory.KickWeapon);
            ProcessHitDetector(collider);
            PushCharacterBack(collider, playerInventory.KickWeapon);
            hitEnemyHealth.TryStun(playerInventory.KickWeapon);
            hitEnemyHealth.AdjustHealth(-dmg);

            canRegisterAttack = false;
            StartCoroutine(TimerForAttackRegistration());
        }
    }

    private void PushCharacterBack(Collider2D collider, WeaponStatFile weaponStats)
    {
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce((rb.transform.position - playerInventory.transform.position).normalized * 500f, ForceMode2D.Impulse);
            Debug.Log("Adding force to enemy");
        }
    }

    private float CalculateDamageAmount(Collider2D collider, Health hitEnemyHealth, WeaponStatFile weaponToUse)
    {
        float dmg = hitEnemyHealth.armored ? weaponToUse.armoredDmg : weaponToUse.baseDmg;
        Transform body = collider.GetComponent<NPCRotator>().body;
        float dot = Vector2.Dot(transform.parent.up, body.up);
        if (dot > 0.2f)
        {
            // Player is behind the target, add extra damage
            dmg *= 1.3f;
        }

        return dmg;
    }

    private void PlayHitSound(WeaponStatFile weaponToUse)
    {
        int length = weaponToUse.availableAttacks[0].hitSounds.Length;
        if (length > 0)
        {
            int index = UnityEngine.Random.Range(0, length);
            knightSounds.PlaySound(weaponToUse.availableAttacks[0].hitSounds[index]);
        }
    }

    private void ProcessHitDetector(Collider2D collider)
    {
        CharHitDetector hitDetector = collider.GetComponent<CharHitDetector>();
        if (hitDetector)
        {
            hitDetector.LastHitBy = transform; // Doing this so the assignment happens before the var is needed inside the function in Health
        }
    }

    IEnumerator TimerForAttackRegistration()
    {
        yield return new WaitForSeconds(minTimeBetweenAttacks);
        canRegisterAttack = true;
    }

    #region Kicking Doors

    async void RegisterBreakableKick(Collider2D collider)
    {
        IBreakable breakable = collider.GetComponent<IBreakable>();
        if (breakable != null)
        {            
            if (breakable != null && breakable.Breakable())
            {
                breakable.BreakWithForce((collider.transform.position - transform.parent.position).normalized * 700f);
            }
        }

        if (collider.gameObject.layer == 14)
        {
            DoorMechanism doorMechanism = collider.GetComponent<DoorMechanism>();
            if (doorMechanism != null && doorMechanism.UnlockableByPlayer)
            {
                doorMechanism.UnlockDoor();
            }
        }
    }

    #endregion
}
