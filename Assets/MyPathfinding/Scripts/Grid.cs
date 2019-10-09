using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool workIn2D;
    public bool displayGridGizmos = true;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    [Range(0.05f, 5f)]
    public float nodeRadius;
    public TerrainType[] walkableRegions;
    Node[,] grid;
    [Space]
    [Header("TEST STUFF")]
    public Transform playerTransform;

    Dictionary<int, int> walkableRegionsDictionary = new Dictionary<int, int>();
    LayerMask walkableMask;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    #region Start / Awake

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType tType in walkableRegions)
        {
            walkableMask |= tType.terrainMask;
            walkableRegionsDictionary.Add((int)Mathf.Log(tType.terrainMask.value, 2), tType.terrainPenalty);
        }

        GenerateGrid();
    }

    #endregion

    #region Generating Grid

    public void GenerateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeftCorner = transform.position;

        if (workIn2D)
        {
            bottomLeftCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        }
        else
        {
            bottomLeftCorner = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodeWorldPos = Vector3.zero;
                bool isWalkable = true;
                if (workIn2D)
                {
                    nodeWorldPos = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    isWalkable = !(Physics2D.OverlapCircle(nodeWorldPos, nodeRadius, unwalkableMask));
                }
                else
                {
                    nodeWorldPos = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    isWalkable = !(Physics.CheckSphere(nodeWorldPos, nodeRadius, unwalkableMask));
                }

                int movementPenalty = 0;

                // Raycast to find movement penalty of the layer
                if (isWalkable)
                {
                    
                    if (workIn2D)
                    {
                        RaycastHit2D hit2D;
                        hit2D = Physics2D.Raycast(nodeWorldPos + (Vector3.back * 20), Vector3.forward);
                        if (hit2D)
                        {
                            walkableRegionsDictionary.TryGetValue(hit2D.collider.gameObject.layer, out movementPenalty);
                        }
                    }
                    else
                    {
                        Ray ray = new Ray();
                        RaycastHit hit;
                        ray = new Ray(nodeWorldPos + (Vector3.up * 50), Vector3.down);
                        if (Physics.Raycast(ray, out hit, 100f, walkableMask))
                        {
                            walkableRegionsDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                        }
                    }                                     
                }

                grid[x, y] = new Node(isWalkable, nodeWorldPos, x, y, movementPenalty);
            }
        }
    }

    public int MaxSize
    {
        get => gridSizeX * gridSizeY;
    }

    #endregion

    #region Computing Path

    public Node NodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX;
        float percentY;
        if (workIn2D)
        {
            percentX = ( worldPosition.x + gridWorldSize.x / 2 ) / gridWorldSize.x; // Half the board (world's 0,0) + position dividedby full width to get percentage
            percentY = ( worldPosition.y + gridWorldSize.y / 2 ) / gridWorldSize.y;
        }
        else
        {
            percentX = ( worldPosition.x + gridWorldSize.x / 2 ) / gridWorldSize.x;
            percentY = ( worldPosition.z + gridWorldSize.y / 2 ) / gridWorldSize.y;
        }

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    #endregion

    #region Gizmos

    public List<Node> path = new List<Node>();
    private void OnDrawGizmos()
    {       
        if (!workIn2D)
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        }
        else
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));
        }

        if (displayGridGizmos && grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));

            }
        }     
    }

    #endregion

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}
