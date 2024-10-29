using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, CharacterAttack
{
    [SerializeField] private Collider meleeTrigger;
    
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackDelay = 2;
    
    private Animator _animator;
    private bool _attacking;
    private bool _blocking;

    private Transform _transform;
    private Transform _playerTransform;
    
    void Start()
    {
        _transform = transform;
        _playerTransform = GameObject.Find("Player").transform;
        _animator = GetComponent<Animator>();
        
        meleeTrigger.enabled = false;
    }
    
    void Update()
    {
        if (!_attacking && (_playerTransform.position - _transform.position).sqrMagnitude < attackRange * attackRange)
        {
            _animator.SetTrigger("Attack");
            _attacking = true;
            Invoke(nameof(ResetAttack), attackDelay);
        }
    }

    private void ResetAttack()
    {
        _attacking = false;
    }
    
    public bool IsBlocking()
    {
        return _blocking;
    }
    
    public bool IsAttacking()
    {
        return _attacking && (_playerTransform.position - _transform.position).sqrMagnitude < attackRange * attackRange;
    }

    public void OnStartAttack()
    {
        meleeTrigger.enabled = true;
    }
    
    public void OnEndAttack()
    {
        meleeTrigger.enabled = false;
    }
    
}
