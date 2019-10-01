using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NPCRegistry : MonoBehaviour
{
    public static NPCRegistry Instance;

    public NPCDataBase npcDatabase;
    List<GameObject> allNPCInScene = new List<GameObject>();


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Assuming all NPCs are childed to this object
        foreach (Transform child in transform)
        {
            allNPCInScene.Add(child.gameObject);
        }
    }

    #region Enabling / Disabling All NPCs

    public void DisableAllNPCS()
    {
        for (int i = 0; i < allNPCInScene.Count; i++)
        {
            Panda.PandaBehaviour behaviourTree = allNPCInScene[i].GetComponent<Panda.PandaBehaviour>();
            if (behaviourTree)            
                behaviourTree.enabled = false;
            
            NPCRotator rotator = allNPCInScene[i].GetComponent<NPCRotator>();
            if (rotator)
                rotator.EnableRotator(false);

            PolyNav.PolyNavAgent navAgent = allNPCInScene[i].GetComponent<PolyNav.PolyNavAgent>();
            if (navAgent)
                navAgent.enabled = false;

        }
    }

    public void EnableAllNPCS()
    {
        for (int i = 0; i < allNPCInScene.Count; i++)
        {
            Panda.PandaBehaviour behaviourTree = allNPCInScene[i].GetComponent<Panda.PandaBehaviour>();
            if (behaviourTree)
                behaviourTree.enabled = true;

            NPCRotator rotator = allNPCInScene[i].GetComponent<NPCRotator>();
            if (rotator)
                rotator.EnableRotator(true);

            PolyNav.PolyNavAgent navAgent = allNPCInScene[i].GetComponent<PolyNav.PolyNavAgent>();
            if (navAgent)
                navAgent.enabled = true;
        }
    }

    #endregion

    #region Registering / DeRegistering NPCs

    public void RegisterNPC(GameObject npc)
    {
        if (!allNPCInScene.Contains(npc))
            allNPCInScene.Add(npc);
    }

    public void DeRegisterNPC(GameObject npc)
    {
        if (allNPCInScene.Contains(npc))
        {
            allNPCInScene.Remove(npc);
            allNPCInScene.TrimExcess();
        }
    }

    #endregion
}
