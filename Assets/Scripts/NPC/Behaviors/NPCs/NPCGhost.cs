using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Unit))]
public class NPCGhost : MonoBehaviour
{
    private Unit _unit;
    [SerializeField] private List<Transform> players;
    [Tooltip("The time interval (in seconds) at which we select our target.")]
    [SerializeField] private float targetSelectionInterval = 2f;
    [SerializeField] private float targetSelectionIntervalMax = 3f;
    
    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        StartCoroutine(ComputeSelectTargetCo());
    }

    private IEnumerator ComputeSelectTargetCo()
    {
        while (true)
        {
            // Find closest player.
            int playerCount = players.Count;
            if (playerCount != 0)
            {
                float closestDistance = Mathf.Infinity;
                var indexOfClosestPlayer = 0;
                for (int i = 0; i < playerCount; i++)
                {
                    float currentDist = (players[i].position - transform.position).magnitude;
                    if (currentDist < closestDistance)
                    {
                        closestDistance = currentDist;
                        indexOfClosestPlayer = i;
                    }
                }
            
                _unit.SetTarget(players[indexOfClosestPlayer]);
                
                yield return new WaitForSeconds(Random.Range(this.targetSelectionInterval, this.targetSelectionIntervalMax));
            }
            else
            {
                yield return null;
            }
            
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void AddPlayer(Transform playerTransform)
    {
        this.players.Add(playerTransform);
        StopCoroutine(ComputeSelectTargetCo());
        StartCoroutine(ComputeSelectTargetCo());
    }
    
    public void RemovePlayer(Transform playerTransform)
    {
        this.players.Remove(playerTransform);
        StopCoroutine(ComputeSelectTargetCo());
        StartCoroutine(ComputeSelectTargetCo());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.Die();
        }
    }
}
