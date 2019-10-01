using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerHouse : MonoBehaviour
{
    [SerializeField] Transform houseBackroomPlayerSpawn;
    [SerializeField] List<Transform> npcPositionsForRespawn = new List<Transform>();

    [SerializeField] DoorMechanism backDoorMechanism;
    [SerializeField] DoorMechanism frontDoorMechanism;
    BuildingPresenceTrigger presenceTrigger;

    public Transform HouseBackroomSpawn => houseBackroomPlayerSpawn;
    public List<Transform> NpcPositionsForRespawn => npcPositionsForRespawn;

    private void Start()
    {
        if (!houseBackroomPlayerSpawn)
        {
            Debug.LogError("No house backroom spawn point defined in the PlayerHouse script!");
        }

        presenceTrigger = GetComponentInChildren<BuildingPresenceTrigger>();
    }

    public void FadeMainRoof(bool fadeIn)
    {
        if (fadeIn)
            presenceTrigger?.FadeIn();
        else
            presenceTrigger?.FadeOut();
    }

    public void ControlFrontDoor(bool unlock)
    {
        if (unlock)
            frontDoorMechanism?.UnlockDoor();
        else
            frontDoorMechanism?.LockDoor();
    }

    public async void ControlBackDoor(bool unlock)
    {
        if (unlock)
            await backDoorMechanism?.UnlockDoor();
        else
            await backDoorMechanism?.LockDoor();
    }
}
