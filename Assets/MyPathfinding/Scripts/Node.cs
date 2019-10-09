using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gCost;
    public int hCost;
    public int movementPenalty;
    int heapIndex;
    public int fCost
    {
        get => gCost + hCost;
    }
    public int HeapIndex
    {
        get => heapIndex;
        set => heapIndex = value;
    }

    public int gridX;
    public int gridY;
    public Node parent;

    public Node (bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _penalty)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;

        gridX = _gridX;
        gridY = _gridY;
        movementPenalty = _penalty;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int comparison = fCost.CompareTo(nodeToCompare.fCost);
        if (comparison == 0)
        {
            comparison = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -comparison;
    }
}
