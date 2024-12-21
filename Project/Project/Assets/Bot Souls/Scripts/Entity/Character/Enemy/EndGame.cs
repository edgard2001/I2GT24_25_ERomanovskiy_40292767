using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private GameManager _gameManager;
    private Health _health;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        _health = GetComponentInChildren<Health>();
        
        _health.Died += () =>
        {
            Invoke(nameof(GoToMenu), 5);
        };
    }
    
    void GoToMenu()
    {
        _gameManager.LoadMainMenu();
    }
}
