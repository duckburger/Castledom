using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    public bool isOn = false;
    Collider2D collider;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
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

        if (other.GetComponent<Health>() != null) //TODO: Add friend for check
        {
            other.GetComponent<Health>().AdjustHealth(-10); // TODO: Set attack value based on weapon
        }
    }
}
