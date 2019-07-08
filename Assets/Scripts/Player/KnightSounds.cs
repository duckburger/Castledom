using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSounds : MonoBehaviour
{
    KnightAttackController knightAttackSystem;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        knightAttackSystem = GetComponent<KnightAttackController>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayCurrentPreSwingSound()
    {
        PlaySound(knightAttackSystem.CurrentPreSwingAudioClip);
    }

    public void PlayCurrentSwingSound()
    {
        PlaySound(knightAttackSystem.CurrentSwingAudioClip);
    }

   

}
