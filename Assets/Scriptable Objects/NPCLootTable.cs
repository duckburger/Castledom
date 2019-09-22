using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NPC Loot Table", menuName ="Knight Stuff/NPC Loot Table")]
public class NPCLootTable : ExtendedScriptableObject
{
    public GameObject coinsItemPrefab;

    #region Money Drops

    public float minCoinDrop;
    public float maxCoinDrop;

    public float GetMoneyDrop()
    {
        return Mathf.Round(Random.Range(minCoinDrop, maxCoinDrop));
    }

    #endregion
}
