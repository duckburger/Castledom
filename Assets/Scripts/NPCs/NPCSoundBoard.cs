using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundBoard : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] AudioClip[] damagedSounds;
    [SerializeField] AudioClip[] deathSounds;
    Health healthController;
    NPCInventory npcInventory;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
            Debug.LogError($"Not Audio Source connected to {gameObject.name}");
        healthController = GetComponent<Health>();
        npcInventory = GetComponent<NPCInventory>();
    }

    private void OnEnable()
    {
        if (!healthController)
            healthController = GetComponent<Health>();
        healthController.onHealthDecreased += PlayRandomHurtSound;
        healthController.onDied += PlayRandomDeathSound;
    }

    private void OnDisable()
    {
        if (!healthController)
            healthController = GetComponent<Health>();
        healthController.onHealthDecreased -= PlayRandomHurtSound;
        healthController.onDied -= PlayRandomDeathSound;
    }

    void PlayRandomHurtSound()
    {
        int index = Random.Range(0, damagedSounds.Length);
        audioSource.clip = damagedSounds[index];
        audioSource.Play();
    }

    void PlayRandomDeathSound()
    {
        int index = Random.Range(0, deathSounds.Length);
        audioSource.clip = deathSounds[index];
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
    }

    public void PlayCurrentSwingSound()
    {
        if (!npcInventory)
        {
            Debug.LogError($"No NPCInventory found on {gameObject.name}");
            return;
        }
        if (npcInventory.npcWeapon && npcInventory.npcWeapon.swingSounds.Length > 0)
        {
            int index = Random.Range(0, npcInventory.npcWeapon.swingSounds.Length);
            audioSource.clip = npcInventory.npcWeapon.swingSounds[index];
            audioSource.Play();
        }        
    }

    public void PlayCurrentHitSound()
    {
        if (!npcInventory)
        {
            Debug.LogError($"No NPCInventory found on {gameObject.name}");
            return;
        }
        if (npcInventory.npcWeapon && npcInventory.npcWeapon.hitSounds.Length > 0)
        {
            int index = Random.Range(0, npcInventory.npcWeapon.hitSounds.Length);
            audioSource.clip = npcInventory.npcWeapon.hitSounds[index];
            audioSource.Play();
        }        
    }


}
