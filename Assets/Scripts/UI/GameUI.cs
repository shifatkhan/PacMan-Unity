using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text playerNameLabel;
    [SerializeField] private string _mainMenuScene;
    
    private void Awake() => this.playerNameLabel.text = PhotonNetwork.NickName;

    private void LoadMainMenuScene() => SceneManager.LoadScene(_mainMenuScene);
    
    public override void OnLeftRoom() => LoadMainMenuScene();
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
