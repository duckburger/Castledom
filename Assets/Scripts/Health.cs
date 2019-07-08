using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;

    public Action downToFirstHealthThreshold;
    public Action downToSecondHealthThreshold;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void AdjustHealth(float amount)
    {
        currentHealth += amount;

        
        // AdjustHealthBar and/or appearance
        if (currentHealth <= 0)
        {
            // No health left
            // Die();
            return;
        }

        if (currentHealth <= maxHealth / 2)
            downToFirstHealthThreshold?.Invoke();

        if (currentHealth <= maxHealth / 3)
            downToSecondHealthThreshold?.Invoke();
    }
}
