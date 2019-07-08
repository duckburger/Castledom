using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool isOn = false;   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOn)
            return;

        if (other.GetComponent<Health>() != null) //TODO: Add friend for check
        {
            other.GetComponent<Health>().AdjustHealth(-10); // TODO: Set attack value based on weapon
        }
    }
}
