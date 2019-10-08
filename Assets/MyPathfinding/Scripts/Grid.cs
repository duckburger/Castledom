﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool workIn2D;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    [Range(0.05f, 5f)]
    public float nodeRadius;
    Node[,] grid;
    [Space]
    [Header("TEST STUFF")]
    public Transform playerTransform;


    float nodeDiameter;
    int gridSizeX, gridSizeY;

    #region Start / Awake

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

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
                Vector3 worldPos = Vector3.zero;
                bool walkable = true;
                if (workIn2D)
                {
                    worldPos = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                    walkable = !(Physics2D.OverlapCircle(worldPos, nodeRadius, unwalkableMask));
                }
                else
                {
                    worldPos = bottomLeftCorner + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    walkable = !(Physics.CheckSphere(worldPos, nodeRadius, unwalkableMask));
                }
                
                grid[x, y] = new Node(walkable, worldPos);
            }
        }
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

    #endregion

    #region Gizmos

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

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }

    #endregion
}