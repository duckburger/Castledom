using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;
public class PlayerHouse : MonoBehaviour
{

    [SerializeField] Transform houseBackroomPlayerSpawn;
    [SerializeField] List<Transform> npcPositionsForRespawn = new List<Transform>();
    [Space]
    [SerializeField] Transform doorParent;
    [SerializeField] DoorMechanism backDoorMechanism;
    [SerializeField] DoorMechanism frontDoorMechanism;
    BuildingPresenceTrigger presenceTrigger;
    [Space]
    [SerializeField] GameObject doorPrefab;

    Vector2 backDoorPosition;
    Vector2 frontDoorPosition;

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

    public async Task ControlBackDoor(bool unlock, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (!backDoorMechanism)
        {
            backDoorMechanism = Instantiate(doorPrefab, backDoorPosition, Quaternion.identity, doorParent).GetComponent<DoorMechanism>();
        }
        else
        {
            if (!backDoorMechanism.gameObject.activeSelf)
                backDoorMechanism.gameObject.SetActive(true);
        }

        if (unlock)
            await backDoorMechanism?.UnlockDoor();
        else
            await backDoorMechanism?.LockDoor();
    }
}
