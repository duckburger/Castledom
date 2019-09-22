using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarsHolder : MonoBehaviour
{
    [SerializeField] ScriptableEvent onPlayerMoneyUpdated;
    [Space]
    public static GlobalVarsHolder Instance;
    public GlobalVars vars;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        ResetMoney();
    }

    void ResetMoney()
    {
        vars.playerGoldCoins = 0;
    }

    public void UpdatePlayerMoney(float amount)
    {
        vars.playerGoldCoins += amount;
        vars.playerGoldCoins = Mathf.Clamp(vars.playerGoldCoins, 0, long.MaxValue);
        onPlayerMoneyUpdated?.RaiseWithData(amount);
    }
}
