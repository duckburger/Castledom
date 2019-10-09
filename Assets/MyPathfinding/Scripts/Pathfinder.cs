using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(Grid))]
public class Pathfinder : MonoBehaviour
{
    [Header("TEST STUFF")]
    public Transform seeker;
    public Transform target;

    Grid grid;

    private void Awake()
    {
        grid = GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        Node startNode = grid.NodeFromWorldPosition(startPosition);
        Node targetNode = grid.NodeFromWorldPosition(targetPosition);

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
                print($"Path found {stopWatch.ElapsedMilliseconds} ms");
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                int movementCostToNeighbour = currentNode.gCost + GetDirectCost(currentNode, neighbour);
                if (movementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = movementCostToNeighbour;
                    neighbour.hCost = GetDirectCost(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour); // Adding all potential nodes to OPEN list here, to sift through at the start of the while loop
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        UnityEngine.Debug.Log("Found path, spitting it out");
        grid.path = path;
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
