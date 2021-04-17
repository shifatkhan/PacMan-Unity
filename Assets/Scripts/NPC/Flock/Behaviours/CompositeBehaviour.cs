using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FilteredFlockBehaviour
{
    public FlockBehaviour[] Behaviours;
    public float[] weights;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock, Vector3 center)
    {
        if (weights.Length != Behaviours.Length)
        {
            Debug.LogError($"Data mismatch in {name}", this);
            return Vector3.zero;
        }
        
        // Compute move.
        Vector3 move = Vector3.zero;
        
        // Iterate through behaviours.
        for (int i = 0; i < Behaviours.Length; i++)
        {
            Vector3 partialMove = Behaviours[i].CalculateMove(agent, context, flock, center) * weights[i];

            if (partialMove != Vector3.zero)
            {
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
