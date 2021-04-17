using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // if no neighbours, return no adjustment (vector.zero)
        if(context.Count == 0)
            return  Vector3.zero;
        
        // Add all points.
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (var item in filteredContext)
            cohesionMove += item.position;

        cohesionMove /= context.Count;
        
        // Create offset from agent position.
        cohesionMove -= agent.transform.position;

        return cohesionMove;
    }
    
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector3 center)
    {
        return CalculateMove(agent, context, flock);
    }
}
