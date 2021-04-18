using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int Score { get; private set; }
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerAudio _playerAudio;

    private void Start()
    {
        Score = 0;
        
        if(_playerMovement == null)
            _playerMovement = GetComponent<PlayerMovement>();
        if (_playerAudio == null)
            _playerAudio = GetComponent<PlayerAudio>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check which pellet we collected.
        Pellet pellet = other.GetComponent<Pellet>();
        if (pellet != null)
        {
            Score++;
            pellet.Collect();
            _playerAudio.PlayCollectAudio();
            return;
        }
        
        PowerPellet powerPellet = other.GetComponent<PowerPellet>();
        if (powerPellet != null)
        {
            Score++;
            powerPellet.Collect();
            _playerAudio.PlayPowerUpAudio();
            _playerMovement.PowerUp();
            return;
        }
    }
}
