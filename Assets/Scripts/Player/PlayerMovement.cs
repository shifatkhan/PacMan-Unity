using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

/// <summary>
/// Class that takes care of the Player's movement.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviourPun
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _powerUpSpeed = 10f;
    [SerializeField] private float _powerUpDuration = 5f;
    private float _currentSpeed;
    [SerializeField] private StartDirection _startDirection;
    
    [Header("Grid movement")]
    [SerializeField] private Transform _movePoint;
    [SerializeField] private float _movePointSmoothing = 0.05f;
    [SerializeField] private float _unwalkableCheckRadius = 0.5f;
    [SerializeField] private LayerMask unwalkableLayer;

    private bool _isMoving = false;
    private Vector3 _lastPosition;
    private NodeGraph _nodeGraph;
    private PlayerAudio _playerAudio;
    private Vector3 _spawnPoint;
    
    public enum StartDirection
    {
        Left = -1,
        Right = 1
    }

    private void Start()
    {
        // Add this player to ghost instances.
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        foreach (var ghost in ghosts)
            ghost.GetComponent<NPCGhost>().AddPlayer(transform);
        
        // We will move this MovePoint ahead of us to check if there's a wall.
        _movePoint.parent = null;
        _nodeGraph = GameObject.FindWithTag("Tile Manager").GetComponent<NodeGraph>();

        _currentSpeed = _moveSpeed;

        _playerAudio = GetComponent<PlayerAudio>();
        _spawnPoint = transform.position;
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        // INPUT.
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        
        // GRID MOVEMENT.
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _currentSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _movePoint.position) <= _movePointSmoothing)
        {
            Vector3 moveOffset = Vector3.zero;
            
            if (Input.GetKey("a") || Input.GetKey("d"))
            {
                moveOffset = new Vector3(horizontalInput, 0, 0).normalized;
                
                // Check if we can walk over next tile.
                Node nodeToTraverse = _nodeGraph.NodeFromWorldPoint(_movePoint.position + moveOffset);
                if(nodeToTraverse.walkable)
                    _movePoint.position += moveOffset;
            }
            else if (Input.GetKey("w") || Input.GetKey("s"))
            {
                moveOffset = new Vector3(0, 0, verticalInput).normalized;
                
                // Check if we can walk over next tile.
                Node nodeToTraverse = _nodeGraph.NodeFromWorldPoint(_movePoint.position + moveOffset);
                if(nodeToTraverse.walkable)
                    _movePoint.position += moveOffset;
            }
            
            // ROTATION.
            if(moveOffset != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(moveOffset, Vector3.up);
        }
    }

    public void ResetMovePoint() => _movePoint.position = transform.position;

    public void PowerUp()
    {
        StartCoroutine(PowerUpCo());
    }

    private IEnumerator PowerUpCo()
    {
        // Speed player up for a short duration.
        _currentSpeed = _powerUpSpeed;

        yield return new WaitForSeconds(_powerUpDuration);
        
        _currentSpeed = _moveSpeed;
    }
    
    public void Die()
    {
        _playerAudio.PlayDieAudio();
        transform.position = _spawnPoint;
        ResetMovePoint();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_movePoint.transform.position, _unwalkableCheckRadius);
    }
}
