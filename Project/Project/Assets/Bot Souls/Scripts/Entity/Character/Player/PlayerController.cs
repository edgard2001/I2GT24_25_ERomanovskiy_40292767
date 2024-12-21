using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float maxSpeed = 10;
        [SerializeField] private float sprintMultiplier = 2f;
        [SerializeField] private float jumpForceMultiplier = 40000;
        [SerializeField] private float gravityMultiplier = 10;
        [SerializeField] private float dashForceMultiplier = 20000f;
        
        [SerializeField] private float sprintStaminaConsumption = 10;
        [SerializeField] private float jumpStaminaConsumption = 20;
        [SerializeField] private float dashStaminaConsumption = 30;
        
        [Header("Input")] [SerializeField] private float controllerStickDeadZone = 0.2f;

        private PlatformingObject _platforming;
        
        private Rigidbody _rigidbody;
        private Transform _playerModelTransform;
        private Transform _cameraTransform;

        private Vector3 _direction = Vector3.forward;
        private bool _prevBlockingInput;

        private float _speed;
        private float _sprintTime;

        private bool _sprinting;
        private bool _grounded;
        private bool _jumping;
        private bool _dead;

        private GameManager _gameManager;
        private Health _health;
        private Stamina _stamina;

        private Animator _animator;
        private CharacterAttack _attackBehaviour;

        private UpgradesMenu _upgradesMenu;
        
        private void Start()
        {
            _platforming = GetComponentInChildren<PlatformingObject>();
            _platforming.OnTouchGround += OnTouchGround;
            _platforming.OnLeaveGround += OnLeaveGround;
            
            _rigidbody = GetComponent<Rigidbody>();
            _playerModelTransform = transform.GetChild(0).transform;
            _cameraTransform = UnityEngine.Camera.main.transform;
            
            _gameManager = GameManager.Instance;
            _health = GetComponentInChildren<Health>();
            _health.Died += () =>
            {
                _dead = true;
                _rigidbody.excludeLayers = LayerMask.GetMask("Player", "Enemy");
                Invoke(nameof(Death), 2);
            };

            _stamina = GetComponentInChildren<Stamina>();
                
            _animator = GetComponentInChildren<Animator>();
            _attackBehaviour = GetComponentInChildren<CharacterAttack>(); 
            
            _upgradesMenu = GetComponentInChildren<UpgradesMenu>();
            
            Physics.gravity = Vector3.down * (9.81f * gravityMultiplier);
            
            Vector3 position = _gameManager.CheckpointPosition;
            if (position != Vector3.zero) transform.position = position;
        }

        private void Update()
        {
            if (_sprinting)
                if (!_stamina.UseStamina(sprintStaminaConsumption * Time.deltaTime))
                {
                    _sprinting = false;
                    _animator.SetBool("Sprinting", false);
                }
            
            ProcessInput();
        }

        private void FixedUpdate()
        {
            MoveAndTurn();
        }

        private void ProcessInput()
        {
            Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            inputVector = ClampInput(inputVector);

            if (_upgradesMenu.InMenu)
                inputVector = Vector2.zero;
            
            if (_grounded)
                _speed = inputVector.magnitude * maxSpeed * (_sprinting ? sprintMultiplier : 1);
                
            CalculateMovementDirection(inputVector);

            if (_upgradesMenu.InMenu)
                return;
            
            if (_grounded && Input.GetButtonDown("Jump") && _stamina.UseStamina(jumpStaminaConsumption))
            {
                _jumping = true;
                _animator.SetTrigger("Jump");
            }

            if (Input.GetButtonDown("AttackM"))
            {
                _attackBehaviour.Attack();
            }

            if (Input.GetAxis("AttackC") > 0.5f)
            {
                _attackBehaviour.Attack();
            }

            bool blocking = Input.GetAxis("Block") > 0.5f;
            if (_prevBlockingInput != blocking)
            {
                if (blocking)
                {
                    _attackBehaviour.Block();
                }
                else
                {
                    _attackBehaviour.UnBlock();
                }
            }

            _prevBlockingInput = blocking;

           
            if (Input.GetButtonUp("Sprint"))
            {
                if (_sprintTime < 0.2f && _stamina.UseStamina(dashStaminaConsumption))
                {
                    _rigidbody.AddForce(_direction * dashForceMultiplier, ForceMode.Impulse);
                }
                _sprintTime = 0;
            }
            else if (Input.GetButton("Sprint"))
            {
                _sprintTime += Time.deltaTime;
            }
            
            if (_stamina.StaminaPercentage > 0)
            {
                _sprinting = Input.GetButton("Sprint");
                _animator.SetBool("Sprinting", _sprinting);
            }
        }

        private Vector2 ClampInput(Vector2 inputVector)
        {
            if (Mathf.Abs(inputVector.x) < controllerStickDeadZone) inputVector.x = 0;
            if (Mathf.Abs(inputVector.y) < controllerStickDeadZone) inputVector.y = 0;
            if (inputVector.sqrMagnitude > 1) inputVector.Normalize();
            return inputVector;
        }

        private void CalculateMovementDirection(Vector2 inputVector)
        {
            _animator.SetFloat("Speed", (_attackBehaviour.IsAttacking() ? 0 : inputVector.magnitude) * maxSpeed / 6);
            if (inputVector.sqrMagnitude == 0 || !_grounded)
                return;

            _direction = Vector3.zero;

            Vector3 cameraRelativeForward = _cameraTransform.forward;
            cameraRelativeForward.y = 0;
            cameraRelativeForward.Normalize();

            Vector3 cameraRelativeRight = _cameraTransform.right;
            cameraRelativeRight.y = 0;
            cameraRelativeRight.Normalize();

            _direction = cameraRelativeRight * inputVector.x + cameraRelativeForward * inputVector.y;
        }

        private void MoveAndTurn()
        {
            if (_speed > 0 && _grounded && !_dead)
                _playerModelTransform.rotation = Quaternion.Lerp(_playerModelTransform.rotation,
                    Quaternion.LookRotation(_direction),
                    10f * Time.fixedDeltaTime);

            if (_attackBehaviour.IsBlocking() || _attackBehaviour.IsAttacking() || _attackBehaviour.Stunned || _dead)
                _direction = Vector3.zero;

            _rigidbody.velocity = _direction * _speed + Vector3.up * _rigidbody.velocity.y;

            if (_jumping)
            {
                _rigidbody.AddForce(Vector3.up * jumpForceMultiplier);
                _jumping = false;
            }
        }

        private void OnTouchGround()
        {
            _animator.SetBool("Grounded", true);
            _grounded = true;
        }

        private void OnLeaveGround()
        {
            _animator.SetBool("Grounded", false);
            _grounded = false;
        }

        private void Death()
        {
            _gameManager.RestartLevel();
        }
    }
}