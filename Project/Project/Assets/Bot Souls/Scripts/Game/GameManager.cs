using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    private MusicManager _musicManager;
    
    private Checkpoint _checkpoint;
    private Vector3 _checkpointPosition;
    public Vector3 CheckpointPosition => _checkpointPosition;

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else 
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _musicManager = MusicManager.Instance;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        _musicManager.PlayMusic(MusicManager.MusicType.Ambient, 0);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        _musicManager.PlayMusic(MusicManager.MusicType.Ambient, 0);
    }
    
    public void SetCheckpoint(Checkpoint checkpoint)
    {
        if (_checkpoint != null)
            _checkpoint.Disable();
        
        _checkpoint = checkpoint;
        _checkpointPosition = checkpoint.transform.position;
    }
}