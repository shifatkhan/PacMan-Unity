using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{
    private Flock _agentFlock;
    public Flock AgentFlock => _agentFlock;

    private Collider _agentCollider;
    public Collider AgentCollider => _agentCollider;

    private void Awake()
    {
        _agentCollider = GetComponent<Collider>();
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }

    public void Initialize(Flock flock)
    {
        _agentFlock = flock;
    }
}
