using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNode : MonoBehaviour
{
    [SerializeField] private List<PlayerNode> _neighborNodes;
    public List<PlayerNode>  NeighborNodes => _neighborNodes;

    [SerializeField] private List<Vector3>  _directionToNeighbors;
    public List<Vector3> DirectionToNeighbors => _directionToNeighbors;

    [SerializeField] private float _checkRadius = 3f;
    
    [SerializeField] private LayerMask _linecastLayerMask;

    public bool AutomaticallyFindNeighbors = true;
    public bool DrawDebugGizmos = true;

    private void Start()
    {
        if (AutomaticallyFindNeighbors)
            SetupNeighbors();

        SetupDirectionToNeighbors();
    }

    /// <summary>
    /// Automatically find the neighbor nodes by raycasting to nodes in range.
    /// </summary>
    private void SetupNeighbors()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, _checkRadius);
        var hitCount = hitColliders.Length;
        for (var i = 0; i < hitCount; i++)
        {
            if(!hitColliders[i].CompareTag("PlayerNode") || hitColliders[i].gameObject == this.gameObject)
                continue;

            RaycastHit hit;
            if(!Physics.Linecast(transform.position, hitColliders[i].transform.position, out hit, _linecastLayerMask))
                _neighborNodes.Add(hitColliders[i].GetComponent<PlayerNode>());
        }
    }

    /// <summary>
    /// Set up the directions from node to node. This will be used by the player to
    /// make a Grid movement (so we can only move on the grid, not freely).
    /// </summary>
    private void SetupDirectionToNeighbors()
    {
        foreach (var neighbor in _neighborNodes)
        {
            Vector3 distance = neighbor.transform.localPosition - transform.localPosition;

            _directionToNeighbors.Add(distance.normalized);
        }
    }

    private void OnDrawGizmos()
    {
        if (_neighborNodes != null && DrawDebugGizmos)
        {
            Gizmos.color = Color.white;
            var numOfNeighbors = _neighborNodes.Count;
            for (var i = 0; i < numOfNeighbors; i++)
            {
                Gizmos.DrawLine(transform.position, _neighborNodes[i].transform.position);
            }
        }
    }
}
