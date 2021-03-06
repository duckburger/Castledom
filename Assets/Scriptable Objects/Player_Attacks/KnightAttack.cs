﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnightAttack", menuName = "Knight Stuff/Attack")]
public class KnightAttack : ExtendedScriptableObject
{
    public List<AudioClip> attackSoundFx = new List<AudioClip>();
    public AudioClip[] hitSounds;
    public int attackSelectorIndex;
}
