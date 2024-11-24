using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;

    private Health _health;
    private Stamina _stamina;
    private CharacterAttack _attackBehaviour;
    private Animator _animator;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private Transform _playerTransform;

    private Vector3 _playerDirection;
    private float _speed;
    private float _turnSpeed;

    private bool _dead;
    private bool _moving;
    private bool _blocked;
    private Collider _collider;

    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackDelay = 2;

    private bool _attacking;
    private float _attackCooldown;

    [SerializeField] private LayerMask mask;

    void Start()
    {
        _health = GetComponentInChildren<Health>();
        _health.Died += () =>
        {
            _animator.SetTrigger("Death");
            _dead = true;
            _rigidbody.excludeLayers = LayerMask.GetMask("Player", "Enemy");
            Invoke(nameof(Death), 5);
        };
        _health.Damaged += () =>
        {
            _attackCooldown = 3;
            _animator.SetTrigger("Impact");
            _animator.ResetTrigger("Attack");
            _animator.SetBool("Attacking", false);
        };

        _stamina = GetComponentInChildren<Stamina>();
        _attackBehaviour = GetComponentInChildren<CharacterAttack>();
        _animator = GetComponentInChildren<Animator>();

        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
        _playerTransform = GameObject.Find("Player").transform;

        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (_dead) return;

        Vector3 playerPositionFlattened = _playerTransform.position;
        playerPositionFlattened.y = 0;

        Vector3 positionFlattened = transform.position;
        positionFlattened.y = 0;

        _playerDirection = (playerPositionFlattened - positionFlattened).normalized;
        _turnSpeed = Mathf.Lerp(360, 120, (Vector3.Dot(_transform.forward, _playerDirection) + 1) / 2);
        _speed = Mathf.Lerp(0, maxSpeed, (Vector3.Dot(_transform.forward, _playerDirection) + 1) / 2);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_playerDirection),
            _turnSpeed * Time.deltaTime);

        if (_attackCooldown > 0) _attackCooldown -= Time.deltaTime;

        if ((_playerTransform.position - _transform.position).sqrMagnitude < attackRange * attackRange)
        {
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, 4f * Time.deltaTime));
            
            if (_attackCooldown <= 0 && _stamina.StaminaPercentage > 0.5)
            {
                _attackBehaviour.Attack();
                _attackCooldown = attackDelay;
            }
            else
            {
                if (_stamina.StaminaPercentage > 0.5)
                    _attackBehaviour.Block();
                else
                    _attackBehaviour.UnBlock();
            }
            _moving = false;
        }
        else
        {
            _attackBehaviour.UnBlock();
            _moving = true;
        }

        _blocked = false;
        _blocked |= Physics.Raycast(transform.position + transform.up, transform.forward, 1, mask);
        _blocked |= Physics.Raycast(transform.position + transform.up, transform.forward + transform.right, 0.6f, mask);
        _blocked |= Physics.Raycast(transform.position + transform.up, transform.forward - transform.right, 0.6f, mask);
        if (_blocked) _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, 4f * Time.deltaTime));
        else _animator.SetFloat("Speed", _playerDirection.magnitude * _speed / 6);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = transform.forward *
                           (!_moving || _attackBehaviour.Stunned || _dead || _attackBehaviour.IsAttacking() 
                            || _blocked || _attackBehaviour.IsBlocking() ? 0 : _speed);
        _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}