using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{
    [SerializeField] LayerMask visionLayerMask;
    [SerializeField] float timeToLoseTarget = 5f;
    [SerializeField] float visionAngle = 45f;
    [SerializeField] float visionDistance = 1.5f;
    [SerializeField] float preAlertDuration = 1f;
    [SerializeField] Transform body;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] bool isOn = true;
    [SerializeField] bool isShowingVisionCone = true;

    public event Action onPreAlerted;
    public event Action onAlerted;
    public event Action onAlertCancelled;
    public event Action onLostSightOfTarget;

    RaycastHit2D losRaycastHit;
    NPCAI aiController;
    Transform player;
    Vector2 leftEdge;
    Vector2 rightEdge;

    float playerAngle;
    Vector2 toPlayer;
    float distToPlayer;
    bool isAlerted = false;
    bool searching = false;

    bool hasLOS = false;
    float losTimer = 0f;

    Coroutine alertCoroutine;

    public bool Searching { get => searching; set => searching = value; }

    private void OnValidate()
    {
        aiController = GetComponentInParent<NPCAI>();
    }

    public void ShowVisionCone(bool enabled)
    {
        isShowingVisionCone = enabled;
    }

    public void ImmediatelyAlertExternal()
    {
        isAlerted = true;
        onAlerted?.Invoke();
        aiController?.SetInCombatExternally(GlobalPlayerController.PlayerHealth);
    }

    private void Start()
    {
        player = GlobalPlayerController.PlayerTransform;
        if (lineRenderer)
            lineRenderer.useWorldSpace = false;
    }

    private void Update()
    {
        if (!isOn || !player)
            return;

        DrawVisionArch();
        DetectPlayer();

        if (isAlerted)
            CheckLineOfSight();
    }

   
    void DrawVisionArch()
    {
        if (!body)
        {
            Debug.LogError("Connect body to NPCVision");
            return;
        }

        leftEdge = Quaternion.Euler(0, 0, -visionAngle / 2) * body.up.normalized * visionDistance;
        rightEdge = Quaternion.Euler(0, 0, visionAngle / 2) * body.up.normalized * visionDistance;

        Debug.DrawRay(body.position, leftEdge);       
        Debug.DrawRay(body.position, rightEdge);

        if (!lineRenderer || !isShowingVisionCone)
        {
            lineRenderer.enabled = false;
            return;
        }

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;
        lineRenderer.positionCount = 5;
        lineRenderer.SetPosition(0, body.localPosition);
        lineRenderer.SetPosition(1, leftEdge);
        lineRenderer.SetPosition(2, body.up.normalized * visionDistance);
        lineRenderer.SetPosition(3, rightEdge);
        lineRenderer.SetPosition(4, body.localPosition);
    }

    private void DetectPlayer()
    {
        if (!player)
        {
            Debug.LogError("No player found");
            return;
        }

        toPlayer = player.position - body.position;
        distToPlayer = toPlayer.magnitude;
        playerAngle = Vector2.SignedAngle(body.up.normalized, toPlayer.normalized);


        if (distToPlayer <= visionDistance && !isAlerted)
        {
            if (visionAngle / 2 > playerAngle || -(visionAngle / 2) < playerAngle)
            {
                isAlerted = true;
                onPreAlerted?.Invoke();
                alertCoroutine = StartCoroutine(AlertTimer());
            }            
        }

        if (alertCoroutine != null && !aiController.InCombat && !searching)
        {
            if (isAlerted && visionAngle / 2 < playerAngle || -(visionAngle / 2) > playerAngle || distToPlayer > visionDistance)
            {
                isAlerted = false;
                StopCoroutine(alertCoroutine);
                onAlertCancelled?.Invoke();
            }            
        }
    }

    IEnumerator AlertTimer()
    {
        yield return new WaitForSeconds(preAlertDuration);
        if (isAlerted)
        {
            onAlerted?.Invoke();
            aiController?.SetInCombatExternally(GlobalPlayerController.PlayerHealth);
        }
        else
        {
            onAlertCancelled?.Invoke();
        }
    }

    void CheckLineOfSight()
    {
        losRaycastHit = Physics2D.Raycast(body.position, toPlayer, visionDistance * 4, visionLayerMask);
        if (losRaycastHit && losRaycastHit.collider.gameObject.layer == 10) // Checking only for player right now
        {
            hasLOS = true;
            losTimer = 0;
        }
        else
        {
            hasLOS = false;
            losTimer += Time.deltaTime;
            if (losTimer > timeToLoseTarget)
            {
                onLostSightOfTarget?.Invoke();
                aiController?.LoseSightOfCombatTarget();
                ShowVisionCone(true);
                Debug.Log("Lost sight of player");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (hasLOS && body && player)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(body.position, player.position);
        }
        else if (body && player)
        {
            if (losRaycastHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(body.position, losRaycastHit.point);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(body.position, player.position);
            }
        }
            
    }

}
