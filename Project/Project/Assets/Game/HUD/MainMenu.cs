using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    
    void Start()
    {
        _gameManager = GameManager.GetInstance();
    }

    public void StartGame()
    {
        _gameManager.LoadMainScene();
    }
}
