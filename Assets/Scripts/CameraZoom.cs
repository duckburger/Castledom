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
    CinemachineFramingTransposer transposer;
    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_CameraDistance = defaultZoom;
    }

    private void FixedUpdate()
    {
        transposer.m_CameraDistance =  Mathf.Lerp(transposer.m_CameraDistance, transposer.m_CameraDistance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Time.deltaTime * lerpSpeed) ;     
        transposer.m_CameraDistance = Mathf.Clamp(transposer.m_CameraDistance, maxZoom, minZoom);  
    }

}
