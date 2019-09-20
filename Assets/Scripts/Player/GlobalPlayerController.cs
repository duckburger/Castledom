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
    Rigidbody2D rigidBody;
    KnightInteractionDetector interactionDetector;
    CircleCollider2D interactionCollider;

    private void Start()
    {
        if (player)
        {
            movement = player.GetComponent<KnightMovement>();
            rotation = player.GetComponent<KnightRotation>();
            sounds = player.GetComponent<KnightSounds>();
            attacks = player.GetComponent<KnightAttackController>();
            rigidBody = player.GetComponent<Rigidbody2D>();
            interactionDetector = player.GetComponentInChildren<KnightInteractionDetector>();
            interactionCollider = interactionDetector.GetComponent<CircleCollider2D>();
        }
    }

    public void TurnOnPlayerControls()
    {
        movement.enabled = true;
        rotation.enabled = true;
        sounds.enabled = true;
        attacks.enabled = true;
        rigidBody.simulated = true;
        interactionDetector.enabled = true;
        interactionCollider.enabled = true;
    }

    public void TurnOffPlayerControls()
    {
        movement.enabled = false;
        rotation.enabled = false;
        sounds.enabled = false;
        attacks.enabled = false;
        rigidBody.simulated = false;
        rigidBody.velocity = Vector2.zero;
        interactionDetector.enabled = false;
        interactionCollider.enabled = false;
    }

}
