using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static GameManager GetInstance()
    {
        return _instance;
    }

    public void LoadMainScene()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(1);
    }

    public void RestartGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
}