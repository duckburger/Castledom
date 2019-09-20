using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNPCSpawner : MonoBehaviour
{
    public int enemiesInNextWave = 3;
    [SerializeField] GameObject[] regularSoldierPrefabs;
    [Space]
    [SerializeField] Transform npcParent;
    [SerializeField] Transform roadExit;

    public void SpawnWave()
    {
        if (!npcParent)
        {
            Debug.LogError("No NPC parent attached to the Road NPC Spawner");
            return;
        }
        int nextSpawnIndex = Random.Range(0, regularSoldierPrefabs.Length);

        for (int i = 0; i < enemiesInNextWave; i++)
        {
            GameObject newSoldier = Instantiate(regularSoldierPrefabs[nextSpawnIndex], transform.position + Vector3.right * i, Quaternion.identity, npcParent);
            NPCAI aiController = newSoldier.GetComponent<NPCAI>();
            if (!aiController)
            {
                Debug.LogError("No NPCAI scripts found on the newly spawned NPC!");
                return;
            }

            aiController.AssignDestination(roadExit);
        }
    }
}
