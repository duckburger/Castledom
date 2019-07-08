using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVarsHolder : MonoBehaviour
{
    public static GlobalVarsHolder Instance;
    public GlobalVars vars;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
