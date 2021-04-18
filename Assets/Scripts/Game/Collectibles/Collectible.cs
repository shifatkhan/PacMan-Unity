using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int pointsWorth;
    public int PointsWorth => pointsWorth;

    public virtual void Collect()
    {
        Destroy(this.gameObject);
    }
}
