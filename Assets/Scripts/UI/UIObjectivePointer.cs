using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObjectivePointer : MonoBehaviour
{
    [SerializeField] Transform arrow;


    [SerializeField] Transform currentObjective;
    Camera mainCam;
    Vector2 objectiveScreenPosition;
    Vector2 toObjective;
    Vector2 calculatedArrowPosition;
    bool inView = false;

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
        if (!currentObjective)
            return;

        CheckIfObjectiveInView();
        if (!inView)
        {
            arrow.gameObject.SetActive(true);
            UpdateArrowPosition();
        }
        else
        {
            arrow.gameObject.SetActive(false);
            arrow.localPosition = Vector2.zero;
        }
    }

    void CheckIfObjectiveInView()
    {
        objectiveScreenPosition = mainCam.WorldToScreenPoint(currentObjective.position);
        switch (objectiveScreenPosition)
        {
            case Vector2 vec when vec.x > Screen.width || vec.x < 0:
                inView = false;
                break;
            case Vector2 vec when vec.y > Screen.height || vec.y < 0:
                inView = false;
                break;
            default:
                inView = true;
                break;
        }
    }

    void UpdateArrowPosition()
    {
        toObjective = objectiveScreenPosition - (Vector2)mainCam.transform.position;
        calculatedArrowPosition = new Vector2(Mathf.Clamp(toObjective.x, 0, Screen.width), Mathf.Clamp(toObjective.y, 0, Screen.height));
        arrow.position = Vector2.Lerp(arrow.position, calculatedArrowPosition, Time.deltaTime * 5f);
        arrow.transform.up = toObjective;
    }


}
