using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHitDetector : MonoBehaviour
{
    public Transform lastHitBy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17)
        {
            lastHitBy = collision.transform.parent.parent;
        }
    }
}
