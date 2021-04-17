using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] private Text[] _playerNamesText;
    [SerializeField] private Button _startButton;
    private Text _startButtonText;
    
    [SerializeField] private string _gameSceneToLoad;

    private void Awake() => _startButtonText = _startButton.GetComponentInChildren<Text>();

    private void Start() => RefreshUI();

    public void LoadGameScene()
    {
        if(!PhotonNetwork.IsMasterClient)
            Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master Client");
        
        Debug.Log($"PhotonNetwork: Loading level with {PhotonNetwork.CurrentRoom.PlayerCount} players");
        PhotonNetwork.LoadLevel(_gameSceneToLoad);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"OnPlayerEnteredRoom(): {newPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerEnteredRoom IsMasterClient: {PhotonNetwork.IsMasterClient}");
            RefreshUI();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"OnPlayerLeftRoom(): {otherPlayer.NickName}");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"OnPlayerLeftRoom IsMasterClient: {PhotonNetwork.IsMasterClient}");
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        RefreshPlayerNamesUI();
        RefreshButtonUI();
    }
    
    private void RefreshPlayerNamesUI()
    {
        var players = PhotonNetwork.PlayerList;
        var numOfPlayers = players.Length;
        switch (numOfPlayers)
        {
            case 1:
                _playerNamesText[0].text = players[0].NickName;
                _playerNamesText[1].text = "Waiting for player 2...";
                break;
            case 2:
                _playerNamesText[0].text = players[0].NickName;
                _playerNamesText[1].text = players[1].NickName;
                break;
            default:
                _playerNamesText[0].text = "Can't have more than 2 players.";
                _playerNamesText[1].text = "";
                Debug.LogError($"ERROR - LobbyUI: Can't have more that 2 players. There are {numOfPlayers} players.");
                break;
        }
    }

    private void RefreshButtonUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.enabled = PhotonNetwork.PlayerList.Length == 2;
            _startButtonText.text = "Start game!";
        }
        else
        {
            _startButton.enabled = false;
            _startButtonText.text = "Wait for host...";
        }
    }
}
