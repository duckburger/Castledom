using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
public class NPCRotator : MonoBehaviour
{
    public Transform body;
    public Transform legs;
    [Space]
    [SerializeField] float lerpSpeed = 25f;
    PolyNavAgent navAgent;
    float directionAngle;
    NPCAI aiController;

    bool isOn = true;

    private void Start()
    {
        navAgent = GetComponent<PolyNavAgent>();
        aiController = GetComponent<NPCAI>();
    }

    public void EnableRotator(bool enabled)
    {
        isOn = enabled;
    }

    private void Update()
    {
        if (!isOn)
            return;

        if (navAgent.hasPath)
        {
            directionAngle = Mathf.Atan2(navAgent.movingDirection.y, navAgent.movingDirection.x) * Mathf.Rad2Deg - 90f;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
            legs.rotation = Quaternion.Slerp(legs.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
        }     
        else if (aiController && aiController.CombatTarget)
        {
            Vector2 dirToTarget = aiController.CombatTarget.position - transform.position;
            directionAngle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            body.rotation = Quaternion.Slerp(body.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
            legs.rotation = Quaternion.Slerp(legs.rotation, Quaternion.Euler(new Vector3(0, 0, directionAngle)), Time.deltaTime * lerpSpeed);
        }
    }
}
