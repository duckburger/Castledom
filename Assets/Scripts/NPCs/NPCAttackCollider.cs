using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackCollider : AttackCollider
{
    [SerializeField] NPCInventory npcInventory;

    NPCSoundBoard npcSoundBoard;

    private void Start()
    {
        circleCollider = GetComponent<Collider2D>();
        npcSoundBoard = GetComponentInParent<NPCSoundBoard>();

        if (!npcSoundBoard)
            Debug.LogError($"No NPCSoundBoard found on {transform.parent.transform.parent.gameObject.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    public override void HandleCollision(Collider2D collider)
    {
        base.HandleCollision(collider);

        if (!npcInventory)
        {
            Debug.LogError($"Connect NPC inventory to the {transform.parent.gameObject.name} attack collider");
            return;
        }

        Health hitHealth = collider.gameObject.GetComponent<Health>();
        if (hitHealth)
        {
            if (collider.gameObject.layer == 10 || collider.gameObject.layer == 11) // If it's a player or their friendlies (friendlies for having minions, perhaps)
            {
                npcSoundBoard?.PlayCurrentHitSound();
                float dmg = hitHealth.armored ? npcInventory.npcWeapon.armoredDmg : npcInventory.npcWeapon.baseDmg;

                Transform body = collider.transform.GetChild(0); // Assuming first child is ALWAYS the body
                float dot = Vector2.Dot(transform.parent.up, body.up);
                if (dot > 0.2f)
                {
                    // NPC is behind the target, add extra damage
                    dmg *= 2f;
                }
                hitHealth.AdjustHealth(-dmg);
            }
        }
        
    }
}
