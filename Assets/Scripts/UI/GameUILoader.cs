using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUILoader : MonoBehaviour
{
    [SerializeField] private string _gameUIScene;
    
    private void Start() => SceneManager.LoadScene(_gameUIScene, LoadSceneMode.Additive);
}
