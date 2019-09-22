using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon Stat File", menuName ="Knight Stuff/NPC Weapon Stat File")]
public class NPCWeaponStatFile : ExtendedScriptableObject
{
    public float baseDmg;
    public float armoredDmg;
    public AudioClip[] swingSounds;
    public AudioClip[] hitSounds;
}
