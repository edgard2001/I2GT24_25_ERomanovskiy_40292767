using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    private Health _health;
    private CharacterAttack _attackBehaviour;
    
    private Rigidbody _rigidbody;
    private Transform _playerTransform;

    private Vector3 _playerDirection;
    
    void Start()
    {
        _health = GetComponent<Health>();
        _health.OnDeath += () => Destroy(gameObject);
        
        _attackBehaviour = GetComponent<CharacterAttack>();

        _rigidbody = GetComponent<Rigidbody>();
        _playerTransform = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        Vector3 _playerPositionFlattened = _playerTransform.position;
        _playerPositionFlattened.y = 0;

        Vector3 _positionFlattended = transform.position;
        _positionFlattended.y = 0;
        
        _playerDirection = (_playerPositionFlattened - _positionFlattended).normalized;
        _rigidbody.rotation = Quaternion.LookRotation(_playerDirection);
    }
    
    private void FixedUpdate()
    {
        _rigidbody.velocity = _playerDirection * ( _attackBehaviour.IsAttacking() ? 0 : speed);
    }
}
