using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun;

public class TestPUN : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        print("=== Connecting to name server... ===");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public void JoinRandomRoom()
    {
        print("=== Attempting to join a room... ===");
        PhotonNetwork.JoinRandomRoom();
    }
    
    public void CreateRoom()
    {
        print("=== Attempting to create a room... ===");
        PhotonNetwork.CreateRoom("Comp476 test room");
    }
    
    public override void OnConnected()
    {
        print("=== CONNECTED TO NAME SERVER ===");
    }
    
    public override void OnConnectedToMaster()
    {
        print("=== CONNECTED TO MASTER ===");
        JoinRandomRoom();
    }
    
    public override void OnCreatedRoom()
    {
        print("=== ROOM CREATED ===");
    }

    public override void OnJoinedRoom()
    {
        print("=== JOINED ROOM ===");
        PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print($"return code {returnCode}, message: {message}");
        print($"=== Could NOT join a random room. Attempting to create a room instead... ===");
        CreateRoom();
    }
}
