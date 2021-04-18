using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTileManager : MonoBehaviour
{
    public Transform[,] Grid = new Transform[24, 28];
    private GameObject _playerNodes;

    private void Start()
    {
        _playerNodes = GameObject.FindWithTag("PlayerNodes");

        foreach (Transform playerNode in _playerNodes.transform)
        {
            Vector3 nodePosition = playerNode.position;
            Grid[(int)nodePosition.x, (int)nodePosition.z] = playerNode;
        }
    }
}
