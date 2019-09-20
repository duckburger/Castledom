using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [Space]
    public bool armored = false;

    public Action onDownToFirstHealthThreshold;
    public Action onDownToSecondHealthThreshold;
    public Action onHealthDecreased;
    public Action onDied;

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
            onDied?.Invoke();
            return;
        }

        if (amount < 0)
            onHealthDecreased?.Invoke();

        if (currentHealth <= maxHealth / 2)
            onDownToFirstHealthThreshold?.Invoke();

        if (currentHealth <= maxHealth / 3)
            onDownToSecondHealthThreshold?.Invoke();
    }

}
