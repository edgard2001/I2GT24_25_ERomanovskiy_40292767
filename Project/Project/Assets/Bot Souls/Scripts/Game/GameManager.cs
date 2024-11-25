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
    private int _currentGameplayScene;

    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else 
        {
            _instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _currentGameplayScene = 1;
    }

    private void Start()
    {
        _musicManager = MusicManager.Instance;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null) player.transform.position = _checkpointPosition;
        _checkpoint = null;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_currentGameplayScene);
        _musicManager.PlayMusic(MusicManager.MusicType.Ambient, 0);
    }
    
    public void SetCheckpoint(Checkpoint checkpoint)
    {
        if (_checkpoint != null)
            _checkpoint.Disable();
        
        _checkpoint = checkpoint;
        _checkpointPosition = checkpoint.transform.position;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}