using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable 
{
    GameObject BrokenVersion();
    bool Breakable();
    void Break();
    void BreakWithForce(Vector2 incomingForce);
}
