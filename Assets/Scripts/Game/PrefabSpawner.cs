using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public bool SpawnOnStart;
    [SerializeField] private GameObject _prefabToSpawn;
    private void Start()
    {
        if (SpawnOnStart)
            SpawnPrefab();
    }

    public void SpawnPrefab()
    {
        if(!PhotonNetwork.IsMasterClient)
            return;
        
        Vector3 myPosition = transform.position;
        GameObject spawnedObject = 
            PhotonNetwork.Instantiate(_prefabToSpawn.name,
                new Vector3(myPosition.x, _prefabToSpawn.transform.position.y, myPosition.z),
                Quaternion.identity);
        
        spawnedObject.transform.parent = transform;
    }
}
