using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] bool player = false;
    [Space]
    [SerializeField] ScriptableEvent onPlayerDied;
    [SerializeField] ScriptableEvent onPlayerHealthChanged;
    [Space]
    [SerializeField] ScriptableEvent onCombatMoveMade;
    [Space]
    [SerializeField] float currentHealth;
    [SerializeField] float maxHealth = 100f;
    [Space]
    public bool armored = false;

    NPCAI aiController;

    public Action onDownToFirstHealthThreshold;
    public Action onDownToSecondHealthThreshold;
    public Action onHealthDecreased;
    public Action onDied;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        aiController = GetComponent<NPCAI>();
    }

    public void SetHealthDirectly(float newVal)
    {
        currentHealth = newVal;
    }


    public void AdjustHealth(float amount)
    {
        currentHealth += amount;
        CombatMoveInfo moveInfo = new CombatMoveInfo(amount, gameObject);
        onCombatMoveMade?.RaiseWithData(moveInfo);

        if (player)
        {
            CameraControls.Instance?.ScreenShake();
            SendPlayerHealthEvent();
        }

        if (currentHealth <= 0)
        {
            // No health left
            onDied?.Invoke();
            if (player)
                onPlayerDied?.Raise();
            return;
        }

        if (amount < 0)
            onHealthDecreased?.Invoke();

        if (currentHealth <= maxHealth / 2)
            onDownToFirstHealthThreshold?.Invoke();

        if (currentHealth <= maxHealth / 3)
            onDownToSecondHealthThreshold?.Invoke();
    }

    private void SendPlayerHealthEvent()
    {
        UIPlayerHealth.UIHealthData healthData = new UIPlayerHealth.UIHealthData(currentHealth, maxHealth);
        onPlayerHealthChanged?.RaiseWithData(healthData);
    }

    public void TryStun(WeaponStatFile weaponInfo)
    {
        if (weaponInfo.stunProbability <= 0)
            return;

        bool stun = CheckStunChance(weaponInfo.stunProbability);
        if (!stun)
            return;

        aiController?.Stun(true, weaponInfo.stunDuration);
    }

    private bool CheckStunChance(float stunProbability)
    {
        if (aiController.IsStunned)
            return false;

        System.Random rand = new System.Random();
        int chance = rand.Next(1, 101);
        if (chance <= stunProbability)
            return true;
        else
            return false;
    }   
}
