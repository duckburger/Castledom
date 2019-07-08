using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightRotation : MonoBehaviour
{
    [SerializeField] Transform knightBody;
    [SerializeField] Transform knightLegs;
    [Space]
    [SerializeField] float lerpSpeed = 25f;
    Camera mainCam;
    float bodyAngle;
    float inputLegAngle;
    float bodyToLegsAngle;
    float velocity;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        RotateToMouse();  
    }

    void RotateToMouse()
    {       
        if (knightBody)
        {
            RotateBody();
        }        

        bodyToLegsAngle = Quaternion.Angle(knightBody.rotation, knightLegs.rotation);
        // Debug.LogError($"Angle between body and legs is {bodyToLegsAngle}");

        if (knightLegs && Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            RotateLegs();
        }
        else if (knightLegs)
        {            
            AlignLegsWithBody();
        }
    }

    private void RotateBody()
    {
        Vector2 toCursor = Input.mousePosition - mainCam.WorldToScreenPoint(knightBody.position);
        toCursor.Normalize();
        bodyAngle = (Mathf.Atan2(toCursor.y, toCursor.x) * Mathf.Rad2Deg) - 90f;
        knightBody.rotation = Quaternion.Lerp(knightBody.rotation, Quaternion.Euler(new Vector3( 0, 0, Mathf.SmoothDampAngle( knightBody.eulerAngles.z, bodyAngle, ref velocity, 0.1f ) ) ), Time.deltaTime * lerpSpeed);
    }

    private void RotateLegs()
    {
        Vector2 legDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputLegAngle = (Mathf.Atan2(legDirection.y, legDirection.x) * Mathf.Rad2Deg) - 90f;   
        // Debug.LogError($"Input to body angle is {Mathf.DeltaAngle(inputLegAngle, bodyAngle)}");
        if (Mathf.CeilToInt(Mathf.DeltaAngle(inputLegAngle, bodyAngle)) > 70f || Mathf.FloorToInt(Mathf.DeltaAngle(inputLegAngle, bodyAngle)) < -70f)
        {
            knightLegs.rotation = Quaternion.Slerp(knightLegs.rotation, Quaternion.Euler(new Vector3(0, 0, inputLegAngle + 180f)), Time.deltaTime * lerpSpeed);
            // Debug.LogError($"Aligning legs and body in motion");
        }
        else
        {
            knightLegs.rotation = Quaternion.Slerp(knightLegs.rotation, Quaternion.Euler(new Vector3(0, 0, inputLegAngle)), Time.deltaTime * lerpSpeed);
        }
    }

    void AlignLegsWithBody()
    {        
        if (bodyToLegsAngle > 70f)
        {
            // Debug.LogError($"Aligning legs and body in stationary");
            knightLegs.rotation = knightBody.rotation;
        }
    }
}
