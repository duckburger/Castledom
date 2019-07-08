using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHitDetector : MonoBehaviour
{

    [SerializeField] GameObject bloodParticlesPrefab;

    Vector2 direction;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        direction = collider.transform.parent.position - transform.position;
        direction.Normalize();

        GameObject newBloodSplat = Instantiate(bloodParticlesPrefab, transform.position, Quaternion.identity);
        newBloodSplat.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        newBloodSplat.GetComponent<ParticleSystem>().Play();
    }


    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, direction);    
    }

}
