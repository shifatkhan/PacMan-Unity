using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayInGameMenu : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }
}
