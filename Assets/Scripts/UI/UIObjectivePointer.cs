using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform arrow;


    [SerializeField] Transform currentObjective;
    Camera mainCam;
    Vector2 calculatedArrowPosition;

    private void Start()
    {
        mainCam = Camera.main;
    }

    public void AssignObjective(Transform newObjective)
    {
        currentObjective = newObjective;
    }

    private void FixedUpdate()
    {
        UpdateArrow();
    }

    void UpdateArrow()
    {
        Vector2 toObjective = mainCam.WorldToScreenPoint(currentObjective.position) - mainCam.transform.position;
        calculatedArrowPosition = new Vector2(Mathf.Clamp(toObjective.x, 0, Screen.width), Mathf.Clamp(toObjective.y, 0, Screen.height));
        arrow.position = Vector2.Lerp(arrow.position, calculatedArrowPosition, Time.deltaTime * 5f);
    }
}
