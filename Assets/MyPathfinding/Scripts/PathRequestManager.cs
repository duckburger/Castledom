using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

[RequireComponent(typeof(Pathfinder))]
public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentRequest;

    static PathRequestManager Instance;
    Pathfinder pathfinder;
    bool isProcessingPath;

    private void Awake()
    {
        Instance = this;
        pathfinder = GetComponent<Pathfinder>();
    }

    public static async Task<(Vector3[], bool)> RequestPathAsync(Vector3 pathStart, Vector3 pathEnd)
    {
        PathRequest newReq = new PathRequest(pathStart, pathEnd);
        Instance.pathRequestQueue.Enqueue(newReq);
        if (!Instance.isProcessingPath)
            return await Instance.TryProcessNextAsync();

        await Task.Run(() => 
        {
            while (!Instance.pathRequestQueue.Peek().Equals(newReq))
                Task.Delay(10);
        });

        return await Instance.TryProcessNextAsync();
    }

    async Task<(Vector3[], bool)> TryProcessNextAsync()
    {     
        currentRequest = pathRequestQueue.Dequeue();
        isProcessingPath = true;
        (Vector3[], bool) searchResults = await pathfinder.StartFindPathAsync(currentRequest.pathStart, currentRequest.pathEnd);
        return await FinishedProcessingPath(searchResults.Item1, searchResults.Item2);        
    }

    public async Task<(Vector3[], bool)> FinishedProcessingPath(Vector3[] path, bool success)
    {
        isProcessingPath = false;
        if (success)
        {
            return (path, success);
        }        
        else
        {
            return (null, success);
        }
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
        }
    }
}
