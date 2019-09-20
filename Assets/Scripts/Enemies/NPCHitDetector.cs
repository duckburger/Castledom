using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitDetector : MonoBehaviour
{

    [SerializeField] GameObject bloodParticlesPrefab;
    [SerializeField] Sprite deadBodySprite;
    [SerializeField] float knockbackStrength = 35f;

    Transform lastHitBy;
    Vector2 direction;

    private void Start()
    {
        GetComponent<Health>().onDied += Die;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<AttackCollider>())
        {
            BloodSplat(collider.transform);
            lastHitBy = collider.transform;
        }        
    }

    void BloodSplat(Transform transform)
    {
        direction = transform.parent.position - transform.position;
        direction.Normalize();

        GameObject newBloodSplat = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
        newBloodSplat.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        newBloodSplat.GetComponent<ParticleSystem>().Play();
    }


    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, direction);    
    }

    void Die()
    {
        Vector2 dirToHitter = transform.position - lastHitBy.position;
        float angleToHitter = Mathf.Atan2(dirToHitter.y, dirToHitter.x) * Mathf.Rad2Deg;
        // Spawn dead body sprite and direct it away from last attack
        MakeDeadBody(dirToHitter, angleToHitter);
        Destroy(gameObject);
    }

    private void MakeDeadBody(Vector2 dirToHitter, float angle)
    {
        GameObject deadBody = new GameObject($"{gameObject.name}'s corpse", typeof(SpriteRenderer), typeof(Rigidbody2D));
        deadBody.transform.eulerAngles += new Vector3(0, 0, angle - 90f);
        deadBody.transform.position = transform.position;
        SpriteRenderer rend = deadBody.GetComponent<SpriteRenderer>();
        rend.sprite = deadBodySprite;
        rend.sortingOrder = 2;
        Rigidbody2D rb = deadBody.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.angularDrag = 2;
        rb.drag = 5;
        rb.mass = 6;
        rb.AddRelativeForce(dirToHitter * knockbackStrength, ForceMode2D.Impulse); // TODO: Make knockback depend on the weapon so it's more fun
        // Destroy self
    }
}
