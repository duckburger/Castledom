using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraControls : MonoBehaviour
{
    public static CameraControls Instance;

    [SerializeField] CinemachineVirtualCamera virtualCam;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ScreenShake()
    {
        if (!virtualCam)
            return;

        LeanTween.value(0, 0.3f, 0.1f).setLoopPingPong(1).setEase(LeanTweenType.easeShake)
            .setOnUpdate((float val) => 
            {
                virtualCam.m_Lens.Dutch = val;
            });        
    }
}
