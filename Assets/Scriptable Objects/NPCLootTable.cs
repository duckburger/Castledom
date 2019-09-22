using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NPC Loot Table", menuName ="Knight Stuff/NPC Loot Table")]
public class NPCLootTable : ExtendedScriptableObject
{

    #region Money Drops

    public float minCoinDrop;
    public float maxCoinDrop;

    public float GetMoneyDrop()
    {
        return Random.Range(minCoinDrop, maxCoinDrop);
    }

    #endregion
}
