using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
public class Test_SetNavTarget : MonoBehaviour
{
    [SerializeField] PolyNavAgent navAgent;
    [SerializeField] Transform testTarget;


    public void SetTestTarget()
    {
        navAgent.SetDestination(testTarget.position);
    }
}
