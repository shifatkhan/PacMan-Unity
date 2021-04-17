using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Controller (MVC)
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    private const string PlayerNamePrefKey = "PlayerName";

    [SerializeField] private Text playerNameLabel;

    private void Awake()
    {
        string playerNameText = PlayerPrefs.GetString(PlayerNamePrefKey);
        if (!PlayerPrefs.HasKey(PlayerNamePrefKey))
            playerNameText = "No name found";
        
        SetPlayerName(playerNameText);
    }

    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Default name";

        PlayerPrefs.SetString(PlayerNamePrefKey, playerName);
        this.playerNameLabel.text = playerName;
        PhotonNetwork.NickName = playerName;
    }
}
