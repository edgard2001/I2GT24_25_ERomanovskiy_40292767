using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public float speed;
    private PlayerController _player;

    private float leftBound = -15;
    
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (!_player.gameOver)
            transform.Translate(Vector3.left * (Time.deltaTime * speed));
        if (transform.position.x < leftBound) 
            Destroy(gameObject);
    }
}
