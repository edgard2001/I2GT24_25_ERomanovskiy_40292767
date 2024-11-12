using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;
    public int difficulty;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void SetDifficulty()
    {
        gameManager.StartWithDifficulty(difficulty);
    }
}
