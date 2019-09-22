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
    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        mouseWheelVal += Input.GetAxis("Mouse ScrollWheel");
        virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(virtualCam.m_Lens.OrthographicSize, virtualCam.m_Lens.OrthographicSize - mouseWheelVal * zoomSpeed, Time.deltaTime * lerpSpeed) ;
        virtualCam.m_Lens.OrthographicSize = Mathf.Clamp(virtualCam.m_Lens.OrthographicSize, maxZoom, minZoom);  
    }


    private void LateUpdate()
    {
        mouseWheelVal = Mathf.Lerp(mouseWheelVal, 0, Time.deltaTime * 5f);
    }
}
