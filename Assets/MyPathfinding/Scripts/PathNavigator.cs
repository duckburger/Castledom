using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
public class PathNavigator : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    Vector3[] path;
    int targetIndex;

    Coroutine movementCoroutine;

    private async void Start()
    {
        (Vector3[], bool) pathResults = await PathRequestManager.RequestPathAsync(transform.position, target.position);
        OnPathFound(pathResults.Item1, pathResults.Item2);
    }

    void OnPathFound(Vector3[] newPath, bool success)
    {
        if (success)
        {
            path = newPath;
            if (movementCoroutine != null)
                StopCoroutine(movementCoroutine);
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                    yield break;
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);

                if (i == targetIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i - 1], path[i]);
            }
        }
    }
}
