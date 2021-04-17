using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class PlayerMovement : MonoBehaviourPun
{
    private static readonly int walkingParamHash = Animator.StringToHash("isWalking");
    private static readonly int runningParamHash = Animator.StringToHash("isRunning");
    private static readonly int dieParamHash = Animator.StringToHash("isDead");
    
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _turnSpeed = 50f;
    
    private Animator _animator;
    private bool isMoving = false;
    private bool isRunning = false;

    private Transform _mainCamera;
    private CameraWork _cameraWork;

    private Vector3 lastPos;
    
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _mainCamera = Camera.main.transform;
        _cameraWork = this.GetComponent<CameraWork>();
        lastPos = transform.position;
        if (_cameraWork != null)
        {
            if (photonView.IsMine)
                _cameraWork.OnStartFollowing();
        }
        else
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on Player prefab.", this);
    }

    private void Start()
    {
        GameObject hunter = GameObject.FindWithTag("Hunter");
        if(hunter != null)
            hunter.GetComponent<NPCHunter>().AddPlayer(transform);
        
        GameObject ranger = GameObject.FindWithTag("Ranger");
        if(ranger != null)
            ranger.GetComponent<NPCRanger>().AddPlayer(transform);
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // MOVE.
        if (!Mathf.Approximately(horizontalInput, 0.0f) || !Mathf.Approximately(verticalInput, 0.0f))
        {
            //Vector3 direction = new Vector3(horizontalInput, 0.0f, verticalInput);
            Vector3 direction = horizontalInput * _mainCamera.right + verticalInput * _mainCamera.forward;
            direction = new Vector3(direction.x, 0f, direction.z);
            direction.Normalize();

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), _turnSpeed * Time.deltaTime);
            
            _animator.SetBool(walkingParamHash, true);
            isMoving = true;

            lastPos = transform.position;
        }
        else
        {
            _animator.SetBool(walkingParamHash, false);
            _animator.SetBool(runningParamHash, false);
            isMoving = false;
            isRunning = false;
            transform.position = lastPos;
        }
        
        // RUN.
        if (Input.GetButtonDown("Fire3") && isMoving)
        {
            _animator.SetBool(runningParamHash, true);
            isRunning = true;
        }
        else if (Input.GetButtonUp("Fire3") && isRunning)
        {
            _animator.SetBool(runningParamHash, false);
            isRunning = false;
        }
    }

    public void ReceiveDamage()
    {
        _animator.SetTrigger(dieParamHash);
    }
    
    public void Die()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
