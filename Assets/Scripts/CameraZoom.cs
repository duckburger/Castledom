using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 2;
    [SerializeField] float lerpSpeed = 15f;
    [SerializeField] float defaultZoom = 6.6f;
    [SerializeField] float minZoom = 20f;
    [SerializeField] float maxZoom = 3.5f;
    CinemachineVirtualCamera virtualCam;
    float mouseWheelVal;

    bool isOn = true;

    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Enable(bool enable)
    {
        isOn = enable;
    }

    private void Update()
    {
        if (!isOn)
            return;
        mouseWheelVal += Input.GetAxis("Mouse ScrollWheel");
        virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(virtualCam.m_Lens.OrthographicSize, virtualCam.m_Lens.OrthographicSize - mouseWheelVal * zoomSpeed, Time.deltaTime * lerpSpeed) ;
        virtualCam.m_Lens.OrthographicSize = Mathf.Clamp(virtualCam.m_Lens.OrthographicSize, maxZoom, minZoom);
        mouseWheelVal = Mathf.Lerp(mouseWheelVal, 0, Time.deltaTime * 5f);
    }


    public void LerpToVal(float val)
    {
        Enable(false);
        LeanTween.value(virtualCam.m_Lens.OrthographicSize, val, 0.23f).setEase(LeanTweenType.easeInOutSine)
            .setOnUpdate((float value) => 
            {
                virtualCam.m_Lens.OrthographicSize = value;
            });
    }
}
