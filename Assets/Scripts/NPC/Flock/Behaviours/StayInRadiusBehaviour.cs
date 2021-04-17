using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay in radius")]
public class StayInRadiusBehaviour : FilteredFlockBehaviour
{
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector3 center)
    {
        Vector3 centerOffset = center - agent.transform.position;
        float deltaDist = centerOffset.magnitude / radius;
        if (deltaDist < 0.9f)
        {
            return Vector3.zero;
        }

        return centerOffset * deltaDist * deltaDist;
    }
}
