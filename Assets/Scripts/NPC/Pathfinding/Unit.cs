using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform target;
    Vector3[] path;
    int targetIndex;
    public float arrivalDistance = 1f;

    private NPC npc;

    [Tooltip("The time interval (in seconds) at which we compute the path finding.")]
    [SerializeField] private float pathfindingInterval = 2f;
    [SerializeField] private bool keepPathfinding = true;

    [SerializeField] private bool enableDirectPathingWhenNear = false;
    private bool directPathing = false;
    
    private void Awake() => npc = GetComponent<NPC>();

    private void Start()
    {
        directPathing = false;
        enableDirectPathingWhenNear = false;
        
        StartCoroutine(ComputePathfindingCo());
    }

    private void Update()
    {
        if(target == null)
            return;
        
        if ((target.position - transform.position).magnitude <= this.npc.NearRadius && enableDirectPathingWhenNear)
        {
            this.npc.SetTarget(target.position);
            this.directPathing = true;
        }
        else
        {
            this.directPathing = false;
        }
    }

    private IEnumerator ComputePathfindingCo()
    {
        while (this.keepPathfinding)
        {
            if(!this.directPathing && target != null)
                RequestPathFinding();
            
            yield return new WaitForSeconds(this.pathfindingInterval);
        }
    }

    private void RequestPathFinding() => PathRequestManager
        .RequestPath(transform.position, target == null ? transform.position:target.position, OnPathFound);

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;

            int pathLength = path.Length;
            float totalLength = 0;
            for (int i = 0; i < pathLength-1; i++)
            {
                totalLength += (path[i + 1] - path[i]).magnitude;
            }

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path != null && path.Length > 0)
        {
            Vector3 currentWaypoint = path[0];

            while (true)
            {
                if (!this.directPathing)
                {
                    if((currentWaypoint - transform.position).magnitude <= arrivalDistance)
                    {
                        targetIndex++;
                        if (targetIndex >= path.Length)
                        {
                            // Exit out of coroutine since we reached the end.
                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                    }
                    npc.SetTarget(currentWaypoint);
                }
                
                yield return null;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            // Draw path.
            for (int i = targetIndex; i < path.Length; i++)
            {
                // Draw nodes.
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(path[i], 0.5f);

                // Draw lines.
                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
