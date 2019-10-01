using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GlobalPlayerController : MonoBehaviour
{
   [SerializeField] GameObject player;

    Health health;
    KnightMovement movement;
    KnightRotation rotation;
    KnightSounds sounds;
    KnightAttackController attacks;
    Rigidbody2D rigidBody;
    KnightInteractionDetector interactionDetector;
    CircleCollider2D interactionCollider;

    private void Awake()
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
            health = player.GetComponent<Health>();
        }
    }

    #region Controls

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
        if (!player)
        {
            Debug.LogError("Can't find player component in the scene");
            return;
        }
        movement.enabled = false;
        rotation.enabled = false;
        sounds.enabled = false;
        attacks.enabled = false;
        rigidBody.simulated = false;
        rigidBody.velocity = Vector2.zero;
        interactionDetector.enabled = false;
        interactionCollider.enabled = false;
    }

    #endregion

    #region Turning Player Obj On/Off

    public void TurnOnPlayerObject()
    {
        if (player)
            player.gameObject.SetActive(true);
    }

    public void TurnOffPlayerObject()
    {
        if (player)
            player.gameObject.SetActive(false);
    }

    #endregion

    #region Moving Player

    public void MovePlayer(Vector2 newWorldPosition)
    {
        if (player)
            player.transform.position = newWorldPosition;
    }

    #endregion

    #region Player Controls

    public void SetPlayerHealth(float newVal)
    {
        if (!health)
        {
            Debug.LogError("Can't adjust health because didn't find the script on the player");
            return;
        }

        health.SetHealthDirectly(newVal);
    }

    #endregion

}
