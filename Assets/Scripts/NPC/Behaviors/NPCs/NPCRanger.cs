using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(NPC))]
public class NPCRanger : MonoBehaviour
{
    private Unit _unit;
    private NPC _npc;
    [SerializeField] private List<Transform> players;
    [Tooltip("The time interval (in seconds) at which we select our target.")]
    [SerializeField] private float targetSelectionInterval = 10f;
    [Tooltip("Radius in which the ranger will shoot at the player.")]
    [SerializeField] private float lineOfSightRadius = 10f;
    
    [Header("Fire arm")]
    [SerializeField] private float fireRate = 1.5f;
    private float fireTime;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Patrol area")]
    [SerializeField] private Vector3 maxPos;
    [SerializeField] private Vector3 minPos;
    [SerializeField] private GameObject dummyTargetPrefab;
    private Transform dummyTarget;

    private bool attacking = false;
    private int playerTargetIndex = 0;
    
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _npc = GetComponent<NPC>();
        attacking = false;
        playerTargetIndex = 0;
    }

    private void Start()
    {
        StartCoroutine(SelectRandomTargetPosition());
        fireTime = Time.time + fireRate;
    }

    private void Update()
    {
        // foreach (var player in players)
        // {
        //     attacking = (player.position - transform.position).magnitude <= lineOfSightRadius;
        // }

        if (attacking)
        {
            playerTargetIndex = GetIndexOfClosestPlayer();
            AttackPlayer();
        }
    }

    private IEnumerator SelectRandomTargetPosition()
    {
        while (true)
        {
            if (dummyTarget == null)
            {
                this.dummyTarget = Instantiate(dummyTargetPrefab, transform.position, Quaternion.identity).GetComponent<Transform>();
            }
            
            // We'll use maxTransform as a dummy target.
            Vector3 newTargetPosition = new Vector3(Random.Range(minPos.x, maxPos.x), 0f, Random.Range(minPos.z, maxPos.z));
            dummyTarget.position = newTargetPosition;
            _unit.SetTarget(dummyTarget);

            yield return new WaitForSeconds(this.targetSelectionInterval);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private void AttackPlayer()
    {
        if (players[playerTargetIndex] == null)
            return;
        
        transform.rotation = Quaternion.LookRotation(players[playerTargetIndex].position - transform.position, Vector3.up);

        if (Time.time >= fireTime)
        {
            fireTime = Time.time + fireRate;

            Instantiate(bulletPrefab, new Vector3(transform.position.x, spawnPoint.position.y, transform.position.z),
                new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w));
        }
    }

    public void AddPlayer(Transform playerTransform)
    {
        this.players.Add(playerTransform);
        StopCoroutine(SelectRandomTargetPosition());
        StartCoroutine(SelectRandomTargetPosition());
    }
    
    public void RemovePlayer(Transform playerTransform)
    {
        this.players.Remove(playerTransform);
        StopCoroutine(SelectRandomTargetPosition());
        StartCoroutine(SelectRandomTargetPosition());
    }

    private int GetIndexOfClosestPlayer()
    {
        // Find closest player.
        float closestDistance = Mathf.Infinity;
        var indexOfClosestPlayer = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if(players[i] == null)
                continue;
            
            float currentDist = (players[i].position - transform.position).magnitude;
            if (currentDist < closestDistance)
            {
                closestDistance = currentDist;
                indexOfClosestPlayer = i;
            }
        }

        return indexOfClosestPlayer;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        attacking = true;
        _unit.enabled = false;
        _npc.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        attacking = false;
        _unit.enabled = true;
        _npc.enabled = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player != null)
        {
            print("PLAYER COLLIDE WITH ranger");
            player.ReceiveDamage();
        }
    }
}
