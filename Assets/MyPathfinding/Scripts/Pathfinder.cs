using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(Grid))]
public class Pathfinder : MonoBehaviour
{
    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }


    async Task<(Vector3[], bool)> FindPathAsync(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathFound = false;

        Node startNode = grid.NodeFromWorldPosition(startPosition);
        Node targetNode = grid.NodeFromWorldPosition(targetPosition);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize); // Open set is a HEAP because that is more performant instead of a massive comparison loop
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    stopWatch.Stop();
                    pathFound = true;
                    print($"Path found {stopWatch.ElapsedMilliseconds} ms");
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                        continue;

                    int neighbourGCost = currentNode.gCost + GetDirectCost(currentNode, neighbour) + neighbour.movementPenalty; // Determines the current gCost of this neighbour
                    if (neighbourGCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = neighbourGCost;
                        neighbour.hCost = GetDirectCost(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour); // Adding all potential nodes to OPEN list here, to sift through at the start of the while loop
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }

            if (pathFound)
                waypoints = await RetracePath(startNode, targetNode);
            return (waypoints, pathFound);
        }
        return (null, false);
    }

    public async Task<(Vector3[], bool)> StartFindPathAsync(Vector3 pathStart, Vector3 pathEnd)
    {
        return await FindPathAsync(pathStart, pathEnd);
    }

    async Task<Vector3[]> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = await SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    async Task<Vector3[]> SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }


    int GetDirectCost(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
            return 14 * distanceY + 10 * (distanceX - distanceY); // 14 is diagonal cost per tile and 10 is vert/horiz cost per tile+
        else
            return 14 * distanceX + 10 * (distanceY - distanceX);
    }

}
