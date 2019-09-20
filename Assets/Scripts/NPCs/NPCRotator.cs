using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
public class NPCRotator : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] Transform legs;
    [Space]
    [SerializeField] float lerpSpeed = 25f;
    PolyNavAgent navAgent;
    float directionAngle;

    private void Start()
    {
        navAgent = GetComponent<PolyNavAgent>();
    }

    private void Update()
    {
        if (navAgent.hasPath)
        {
            directionAngle = Mathf.Atan2(navAgent.movingDirection.y, navAgent.movingDirection.x) * Mathf.Rad2Deg - 90f;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
            legs.rotation = Quaternion.Slerp(legs.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
        }       
    }
}
