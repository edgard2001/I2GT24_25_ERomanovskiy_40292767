using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameManager _gameManager;
    private MusicManager _musicManager;
    
    private TMP_Text _startPrompt;
    
    void Start()
    {
        _gameManager = GameManager.Instance;
        _musicManager = MusicManager.Instance;
        _musicManager.PlayMusic(MusicManager.MusicType.Menu, 0);
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
