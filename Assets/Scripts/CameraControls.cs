using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraControls : MonoBehaviour
{
    public static CameraControls Instance;

    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] CameraZoom zoomControls;

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

        zoomControls = GetComponentInChildren<CameraZoom>();
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


    #region Zoom Controls

    public void LerpZoomToValue(float value)
    {
        zoomControls?.LerpToVal(value);
    }

    public void EnableZoomControls(bool enabled)
    {
        zoomControls?.Enable(enabled);
    }

    #endregion
}
