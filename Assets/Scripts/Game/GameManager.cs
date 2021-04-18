using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The player prefab to spawn.")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private string _mainMenuScene;

    private void Start()
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. " +
                           "Please set it up in GameObject 'Game Manager'",this);
        }
        else
        {
            // Spawn player.
            if (PlayerNetwork.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                PhotonNetwork.Instantiate(this._playerPrefab.name, _spawnPosition.position, Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"OnPlayerLeftRoom(): {otherPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerLeftRoom IsMasterClient: {PhotonNetwork.IsMasterClient}");
            LeaveRoom();
        }
    }

    private void LoadMainMenuScene() => SceneManager.LoadScene(_mainMenuScene);
    
    public override void OnLeftRoom() => LoadMainMenuScene();
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }
}
