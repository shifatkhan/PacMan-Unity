using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerNetwork : MonoBehaviourPun
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerNetwork.LocalPlayerInstance = this.gameObject;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}
