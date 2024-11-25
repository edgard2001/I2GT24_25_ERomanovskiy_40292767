using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject hologram;
    private GameManager _gameManager;
    private bool _active;
    
    private void Start()
    {
        _gameManager = GameManager.Instance;
        hologram.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_active) return;

        if (other.CompareTag("Player"))
        {
            hologram.SetActive(true);
            _active = true;
            _gameManager.SetCheckpoint(this);
        }
    }

    public void Disable()
    {
        hologram.SetActive(false);
        _active = false;
    }
}
