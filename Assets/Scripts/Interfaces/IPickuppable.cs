using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickuppable
{
    AudioClip PickupSound();
    object GetPickuppableObject();
    bool AutoPickupped();
    GameObject GetWorldObject();
    void TurnOnAttractor();
    void TurnOffAttractor();
}
