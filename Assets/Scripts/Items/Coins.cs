using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, IPickuppable
{
    public float amount;
    public AudioClip[] pickupSounds;
    public bool AutoPickupped()
    {
        return true;
    }

    public AudioClip PickupSound()
    {
        if (pickupSounds.Length > 0)
            return pickupSounds[Random.Range(0, pickupSounds.Length)];
        else
            return null;
    }

    public object GetPickuppableObject()
    {
        return amount;
    }

    public GameObject GetWorldObject()
    {
        return this.gameObject;
    }

    public void TurnOnAttractor() { }

    public void TurnOffAttractor() { }   
}
