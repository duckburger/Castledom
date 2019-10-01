using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns the dead body and provides blood splat effects
public class CharHitDetector : MonoBehaviour
{
    [SerializeField] LayerMask hitByLayers;
    [Space]
    [SerializeField] GameObject bloodParticlesPrefab;
    [SerializeField] Sprite deadBodySprite;
    [SerializeField] float knockbackStrength = 100f; // For now only applies to a dead body
    [SerializeField] bool destroyOnDeath = true;

    Transform lastHitBy; // Used to determine body rotation on death
    Vector2 bloodSplatDirection;
    Health health;

    public Transform LastHitBy
    {
        get => lastHitBy;
        set => this.lastHitBy = value;
    }

    private void Start()
    {
        health = GetComponent<Health>();
        health.onDied += Die;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (hitByLayers == ((1 << collider.gameObject.layer) | hitByLayers))
        {
            BloodSplat(collider.transform);
            lastHitBy = collider.transform;
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 16) // Hit by an item
        {
            if (collision.relativeVelocity.sqrMagnitude > 50f)
            {                
                ItemHitDetector itemHitDetector = collision.gameObject.GetComponent<ItemHitDetector>();
                if (itemHitDetector)
                {
                    lastHitBy = itemHitDetector.lastHitBy;
                }
                else
                {
                    lastHitBy = collision.transform;
                }
                BloodSplat(lastHitBy, 3);
                health?.AdjustHealth(-collision.relativeVelocity.sqrMagnitude / 2);
            }
        }
    }

    /// <summary>
    /// Transform determine the direction of the blood splat
    /// </summary>
    /// <param name="collisionTransform"></param>
    void BloodSplat(Transform collisionTransform, int bloodMultiplier = 1)
    {
        bloodSplatDirection = collisionTransform.position - transform.position;
        bloodSplatDirection.Normalize();

        for (int i = 0; i < bloodMultiplier; i++)
        {
            GameObject newBloodSplat = Instantiate(bloodParticlesPrefab, collisionTransform.position, Quaternion.identity);
            newBloodSplat.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(bloodSplatDirection.y + i, bloodSplatDirection.x + i) * Mathf.Rad2Deg + 180f);
            newBloodSplat.GetComponent<ParticleSystem>().Play();
        }       
    }


    void Die()
    {
        Vector2 dirToHitter = lastHitBy.up;
        float angleToHitter = Mathf.Atan2(dirToHitter.y, dirToHitter.x) * Mathf.Rad2Deg;
        // Spawn dead body sprite and direct it away from last attack
        MakeDeadBody(dirToHitter, angleToHitter);
        if (destroyOnDeath)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void MakeDeadBody(Vector2 dirToHitter, float angle)
    {
        GameObject deadBody = new GameObject($"{gameObject.name}'s corpse", typeof(SpriteRenderer), typeof(Rigidbody2D));
        deadBody.transform.eulerAngles += new Vector3(0, 0, angle - 90f);
        deadBody.transform.position = transform.position;
        SpriteRenderer rend = deadBody.GetComponent<SpriteRenderer>();
        rend.sprite = deadBodySprite;
        rend.sortingOrder = 3;
        Rigidbody2D rb = deadBody.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.angularDrag = 2;
        rb.drag = 5;
        rb.mass = 6;
        rb.AddRelativeForce(dirToHitter * knockbackStrength, ForceMode2D.Impulse); // TODO: Make knockback depend on the weapon so it's more fun
        // Destroy body
    }
}
