using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ManageScenes : MonoBehaviourPunCallbacks
{
    void LoadArena()
    {
        if(!PhotonNetwork.IsMasterClient)
            Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master Client");
        
        Debug.Log($"PhotonNetwork: Loading level > {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.LoadLevel("RoomFor" + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"OnPlayerEnteredRoom(): {newPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerEnteredRoom IsMasterClient: {PhotonNetwork.IsMasterClient}");
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"OnPlayerLeftRoom(): {otherPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"otherPlayer IsMasterClient: {PhotonNetwork.IsMasterClient}");
            LoadArena();
        }
    }
}
