using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Vector3 spawnPos = new Vector3(25, 0, 0);

    public float startDelay = 2;
    public float repeatRate = 0.5f;

    private PlayerController _player;
    void Start()
    {
        InvokeRepeating(nameof(SpawnObstacle), startDelay, 1/repeatRate);
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void SpawnObstacle()
    {
        if (!_player.gameOver)
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
    }

}