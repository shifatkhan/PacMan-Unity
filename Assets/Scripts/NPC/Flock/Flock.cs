using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flock : MonoBehaviour
{
    [Tooltip("The center of which the flock will stay around.")]
    [SerializeField] private Transform flockCenter;
    
    public FlockAgent agentPrefab;
    private List<FlockAgent> _agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(2, 100)] public int startingCount = 5;
    private const float AgentDensity = 1f;

    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    [Range(1f, 100f)] public float neighbourRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float squareNeighbourRadius;
    private float squareAvoidanceRadius;
    public float SquareAvoidanceRadius => squareAvoidanceRadius;

    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = squareNeighbourRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                (Random.insideUnitSphere * startingCount * AgentDensity) + transform.position,
                Quaternion.Euler((Vector3.forward) * Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            _agents.Add(newAgent);
        }
    }

    private void Update()
    {
        foreach (FlockAgent agent in _agents)
        {
            List<Transform> context = GetNearbyObjects(agent);
            Vector3 move = behaviour.CalculateMove(agent, context, this, 
                flockCenter == null ? Vector3.zero : flockCenter.position);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
                move = move.normalized * maxSpeed;
            
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (var contextCollider in contextColliders)
        {
            if(contextCollider != agent.AgentCollider)
                context.Add((contextCollider.transform));
        }

        return context;
    }
}
