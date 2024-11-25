using System;
using System.Collections;
using System.Collections.Generic;
using Environment.Platform;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private MovingPlatform movingPlatform;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private Material inactiveMaterial;
    
    private Animator _animator;
    private Renderer _renderer;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _renderer = transform.GetChild(0).GetComponent<Renderer>();
        movingPlatform.OnStatusChanged += active => _renderer.material = active ? activeMaterial : inactiveMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _animator.SetTrigger("Pressed");
    }
    
    public void ButtonPressed()
    {
        movingPlatform.Toggle();
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, movingPlatform.transform.position, Color.red);
    }
}
