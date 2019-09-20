using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackCollider : MonoBehaviour
{
    [SerializeField] KnightInventory playerInventory;
    public bool isOn = false;

    public Action onHitEnemy;

    KnightSounds knightSounds;
    Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        knightSounds = GetComponentInParent<KnightSounds>();
    }

    public void TurnOn()
    {
        isOn = true;
        collider.enabled = true;
    }

    public void TurnOff()
    {
        isOn = false;
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOn)
            return;

        if (!playerInventory)
        {
            Debug.LogError("Connect player inventory to the player's attack collider");
            return;
        }

        Health hitEnemyHealth = other.GetComponent<Health>();
        if (hitEnemyHealth != null) //TODO: Add friend for check
        {
            int length = playerInventory.EquippedWeapon.availableAttacks[0].hitSounds.Length;
            if (length > 0)
            {
                int index = UnityEngine.Random.Range(0, length);
                knightSounds.PlaySound(playerInventory.EquippedWeapon.availableAttacks[0].hitSounds[index]);
            }
            float dmg = hitEnemyHealth.armored ? playerInventory.EquippedWeapon.armoredDmg : playerInventory.EquippedWeapon.baseDmg;
            hitEnemyHealth.AdjustHealth(-dmg);
        }
    }
}
