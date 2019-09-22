using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInventory : MonoBehaviour
{
    public NPCWeaponStatFile npcWeapon;
    public NPCLootTable npcLootTable;

    private void Start()
    {
        GetComponent<Health>().onDied += DropContainedItems;
    }

    public void DropContainedItems()
    {
        if (!npcLootTable)
        {
            Debug.LogError($"Connect a loot table to the {gameObject.name}");
            return;
        }
        DropMoney();
        DropItems();
    }

    void DropMoney()
    {
        float amountToDrop = npcLootTable.GetMoneyDrop();

    }

    void DropItems()
    {

    }
}
