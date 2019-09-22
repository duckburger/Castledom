using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="GlobalVars", menuName = "Knight Stuff/GlobalVars")]
public class GlobalVars : ExtendedScriptableObject
{
    public float playerGoldCoins;
    [Space]
    public Color halfDamagedColour;
    public Color fullDamagedColour;
    [Space]
    public Color enemyCombatTextColour;
    public Color friendlyCombatTextColour;
    
}
