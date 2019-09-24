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
    [Space]
    public float stunProbability = 0;
    public float stunDuration = 0;
    [Space]
    public Sprite uiIcon;
    public Sprite idleBodySprite;
    [Space]
    public int animationLayerIndex;
    public KnightAttack[] availableAttacks;
    [Space(5)]
    public GameObject prefab; // Used to spawn when a weapon is dropped
}
