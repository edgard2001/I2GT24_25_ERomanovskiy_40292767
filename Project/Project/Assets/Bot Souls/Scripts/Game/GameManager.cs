using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
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
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null) player.transform.position = _checkpointPosition;
        _checkpoint = null;
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(_currentGameplayScene);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
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