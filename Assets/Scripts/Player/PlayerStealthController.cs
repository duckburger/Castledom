using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStealthController : MonoBehaviour
{
    bool isSneaking = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSneaking = true;
        }
    }
}
