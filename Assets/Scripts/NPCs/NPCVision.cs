using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCVision : MonoBehaviour
{   
    [SerializeField] LayerMask visionLayerMask;
    [SerializeField] int visionArchSegments = 20;
    [SerializeField] float timeToLoseTarget = 5f;
    [SerializeField] float visionAngle = 45f;
    [SerializeField] float visionDistance = 1.5f;
    [SerializeField] float preAlertDuration = 1f;
    [SerializeField] Transform body;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] MeshFilter meshFilter;
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
    List<Vector2> archCastPoints = new List<Vector2>();

    float angleToPlayer;
    Vector2 toPlayer;
    float distToPlayer;
    bool isAlerted = false;
    bool searching = false;
    Mesh viewMesh;

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
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        meshFilter.mesh = viewMesh;
    }

    private void Update()
    {
        if (!isOn || !player)
            return;


        DrawVisionArchMesh();
        DetectPlayer();

        if (isAlerted)
            CheckLineOfSightInChase();
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

    void DrawVisionArchMesh()
    {
        float angleStep = visionAngle / visionArchSegments;
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i <= visionArchSegments; i++)
        {
            float angle = -visionAngle / 2 + angleStep * i;
            ViewCastInfo castInfo = ViewCast(angle);
            points.Add(castInfo.point);
        }

        int vertexCount = points.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = body.InverseTransformPoint(points[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private void DetectPlayer()
    {
        return;

        if (!player)
        {
            Debug.LogError("No player found");
            return;
        }

        toPlayer = player.position - body.position;
        distToPlayer = toPlayer.magnitude;
        angleToPlayer = Vector2.SignedAngle(body.up.normalized, toPlayer.normalized);


        if (distToPlayer <= visionDistance && CheckPlayerLineOfSightSimple() && !isAlerted)
        {
            if (visionAngle / 2 > angleToPlayer || -(visionAngle / 2) < angleToPlayer)
            {
                isAlerted = true;
                onPreAlerted?.Invoke();
                alertCoroutine = StartCoroutine(AlertTimer());
            }            
        }

        if (alertCoroutine != null && !aiController.InCombat && !searching)
        {
            if (isAlerted && visionAngle / 2 < angleToPlayer || -(visionAngle / 2) > angleToPlayer || distToPlayer > visionDistance)
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

    bool CheckPlayerLineOfSightSimple()
    {
        losRaycastHit = Physics2D.Raycast(body.position, toPlayer, visionDistance, visionLayerMask);
        if (losRaycastHit && losRaycastHit.collider.gameObject.layer == 10) // Checking only for player right now
        {
            return true;
        }
        else
        {
            return true;
        }
    }

    void CheckLineOfSightInChase()
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

    #region Data model

    ViewCastInfo ViewCast(float angle)
    {
        Vector2 direction = Quaternion.Euler(0, 0, angle) * body.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionDistance, visionLayerMask);
        if (hit)
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, (Vector2)transform.position + direction * visionDistance, visionDistance, angle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    #endregion

}
