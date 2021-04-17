using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Physics layer")]
public class PhysicsLayerFilter : ContextFilter
{
    public LayerMask mask;
    public float detectionDistance;
    
    public override List<Transform> Filter(FlockAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach (var transform in original)
        {
            if (mask == (mask | (1 << transform.gameObject.layer)))
            {
                filtered.Add(transform);
            }
        }

        return filtered;
    }
}
