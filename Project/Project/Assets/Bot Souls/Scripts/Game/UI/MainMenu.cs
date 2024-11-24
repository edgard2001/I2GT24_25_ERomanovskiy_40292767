using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    private TMP_Text _startPrompt;
    
    void Start()
    {
        _gameManager = GameManager.GetInstance();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
            StartGame();
    }
    
    private void StartGame()
    {
        _gameManager.RestartLevel();
    }
}
