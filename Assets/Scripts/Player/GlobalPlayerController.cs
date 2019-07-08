using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerController : MonoBehaviour
{
   [SerializeField] GameObject player;

    KnightMovement movement;
    KnightRotation rotation;
    KnightSounds sounds;
    KnightAttackController attacks;


    private void Start()
    {
        if (player)
        {
            movement = player.GetComponent<KnightMovement>();
            rotation = player.GetComponent<KnightRotation>();
            sounds = player.GetComponent<KnightSounds>();
            attacks = player.GetComponent<KnightAttackController>();
        }
    }

    public void TurnOnPlayerControls()
    {
        movement.enabled = true;
        rotation.enabled = true;
        sounds.enabled = true;
        attacks.enabled = true;
    }

    public void TurnOffPlayerControls()
    {
        movement.enabled = false;
        rotation.enabled = false;
        sounds.enabled = false;
        attacks.enabled = false;
    }
   
}
