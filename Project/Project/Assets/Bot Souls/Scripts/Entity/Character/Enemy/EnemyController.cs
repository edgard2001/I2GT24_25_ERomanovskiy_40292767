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

    [SerializeField] private float chaseRange = 10;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float attackDelay = 2;

    private bool _attacking;
    private float _attackCooldown;

    [SerializeField] private LayerMask obstacleAvoidanceMask;
    [SerializeField] private LayerMask playerInSightMask;
    
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
    }

    private void Update()
    {
        if (_dead) return;

        bool playerInSight = Physics.Raycast(_transform.position + transform.up,
            _playerTransform.position - transform.position, out RaycastHit hit,
            chaseRange, playerInSightMask);
        //Debug.DrawLine(_transform.position + transform.up, hit.point, Color.red);
        if (!playerInSight || !hit.collider.CompareTag("Player"))
        {
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, 4f * Time.deltaTime));
            _moving = false;
            return;
        }

        Vector3 playerPositionFlattened = _playerTransform.position;
        playerPositionFlattened.y = 0;

        Vector3 positionFlattened = transform.position;
        positionFlattened.y = 0;

        _playerDirection = (playerPositionFlattened - positionFlattened).normalized;
        _turnSpeed = Mathf.Lerp(360, 120, (Vector3.Dot(_transform.forward, _playerDirection) + 1) / 2);
        _speed = Mathf.Lerp(maxSpeed/2, maxSpeed, (Vector3.Dot(_transform.forward, _playerDirection) + 1) / 2);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(_playerDirection),
            _turnSpeed * Time.deltaTime);

        float sqrDistance = (_playerTransform.position - _transform.position).sqrMagnitude;
        if (sqrDistance > chaseRange * chaseRange)
        {
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, 4f * Time.deltaTime));
            _moving = false;
            return;
        }
            
        if (sqrDistance < attackRange * attackRange)
        {
            if (_attackCooldown > 0) _attackCooldown -= Time.deltaTime;
            
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, 4f * Time.deltaTime));
            
            if (_attackCooldown <= 0 && _stamina.StaminaPercentage > 0.25)
            {
                _attackBehaviour.Attack();
                _attackCooldown = attackDelay;
            }
            else if (_attackCooldown <= attackDelay / 2)
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

            Vector3 repelDirection = Vector3.zero;
            Collider[] colliders = Physics.OverlapSphere(_transform.position, 1, obstacleAvoidanceMask);
            foreach (Collider collider in colliders)
            {
                if ((collider.transform.position - _transform.position).y < 0) continue;
                Debug.DrawLine(_transform.position + Vector3.up, collider.ClosestPoint(_transform.position + Vector3.up), Color.red);
                repelDirection += (_transform.position + Vector3.up - collider.ClosestPoint(_transform.position + Vector3.up)).normalized;
            }

            if (repelDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(repelDirection),
                    360 * Time.deltaTime);
            }
        }
        
        if (_moving) _animator.SetFloat("Speed", _playerDirection.magnitude * _speed / 6);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = transform.forward *
                           (!_moving || _attackBehaviour.Stunned || _dead || _attackBehaviour.IsAttacking() 
                             || _attackBehaviour.IsBlocking() ? 0 : _speed);
        _rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}