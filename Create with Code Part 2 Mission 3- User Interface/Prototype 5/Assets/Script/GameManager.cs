using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>(4);
    public float spawnRate = 1.0f;

    private int score;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public GameObject gameTitleScreen;

    public bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator SpawnTarget()
    {
        while (isActive)
        {
            yield return new WaitForSeconds(1 / spawnRate);
            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        isActive = false;
        gameOverScreen.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartWithDifficulty(int difficulty)
    {
        gameTitleScreen.SetActive(false);
        spawnRate *= difficulty;
        StartCoroutine(SpawnTarget());
    }
}
