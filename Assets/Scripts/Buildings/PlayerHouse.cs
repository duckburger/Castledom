using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHouse : MonoBehaviour
{
    [SerializeField] Transform houseBackroomSpawn;
    [SerializeField] BuildingPresenceTrigger presenceTrigger;

    public Transform HouseBackroomSpawn => houseBackroomSpawn;

    private void Start()
    {
        if (!houseBackroomSpawn)
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
}
