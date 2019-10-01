using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDatabase", menuName = "Knight Stuff/NPC Database")]
public class NPCDataBase : ExtendedScriptableObject
{
    [SerializeField] List<GameObject> tier1Enemies = new List<GameObject>();

    public GameObject GetRandomEnemy(int tier)
    {
        switch (tier)
        {
            case 1:
                return tier1Enemies[Random.Range(0, tier1Enemies.Count)];
                break;            
            default:
                break;
        }
        return null;
    }
}
