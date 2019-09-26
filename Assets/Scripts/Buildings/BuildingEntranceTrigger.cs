using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BuildingEntranceTrigger : MonoBehaviour
{
    public Action entranceTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
            entranceTriggered?.Invoke();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
            entranceTriggered?.Invoke();
    }
}
