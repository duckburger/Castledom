using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnightAttack", menuName = "Knight Stuff/Attack")]
public class KnightAttack : ScriptableObject
{
    public List<AudioClip> attackSoundFx = new List<AudioClip>();
    public int animationLayerIndex;
    public int[] attackIndices;
}
