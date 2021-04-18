using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int pointsWorth;
    public int PointsWorth => pointsWorth;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }
    
    [PunRPC]
    public void Collect()
    {
        Destroy(this.gameObject);
    }
}
