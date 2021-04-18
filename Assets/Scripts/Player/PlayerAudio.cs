using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] collectClips;
    [SerializeField] private AudioClip[] powerUpClips;
    [SerializeField] private AudioClip dieClip;
    [Header("Mixer Groups")]
    [SerializeField] private AudioSource audioSource;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void PlayCollectAudio()
    {
        var index = Random.Range(0, this.collectClips.Length);

        this.audioSource.PlayOneShot(this.collectClips[index]);
    }
    
    public void PlayPowerUpAudio()
    {
        var index = Random.Range(0, this.powerUpClips.Length);

        this.audioSource.PlayOneShot(this.powerUpClips[index]);
    }

    public void PlayDieAudio()
    {
        this.audioSource.PlayOneShot(this.dieClip);
    }
}
