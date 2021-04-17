using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FilteredFlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        // if no neighbours, return no adjustment (vector.zero)
        if(context.Count == 0)
            return  Vector3.zero;
        
        // Add all points.
        Vector3 avoidanceMove = Vector3.zero;
        int numToAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (var item in filteredContext)
        {
            Vector3 closestPoint = item.GetComponent<Collider>().ClosestPoint(agent.transform.position);
            if (Vector3.SqrMagnitude(closestPoint - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numToAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }

        if (numToAvoid > 0)
            avoidanceMove /= numToAvoid;

        return avoidanceMove;
    }

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector3 center)
    {
        return CalculateMove(agent, context, flock);
    }
}
