using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (var transform in original)
        {
            FlockAgent itemAgent = transform.GetComponent<FlockAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
                filtered.Add(transform);
        }

        return filtered;
    }
}
