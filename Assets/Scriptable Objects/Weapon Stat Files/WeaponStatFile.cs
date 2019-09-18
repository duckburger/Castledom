using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatFile", menuName = "Knight Stuff/Weapon Stat File")]
public class WeaponStatFile : ExtendedScriptableObject
{
    public string weaponName;
    [Space]
    public float baseDmg;
    public float armoredDmg;
    public Sprite uiIcon;
    [Space]
    public KnightAttack[] availableAttacks;
    [Space(5)]
    public GameObject prefab; // Used to spawn when a weapon is dropped
}
