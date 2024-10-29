using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 10;
        [SerializeField] private float jumpForceMultiplier = 200000;
        [SerializeField] private float gravityMultiplier = 10;
        
        [Header("Input")]
        [SerializeField] private float controllerStickDeadZone = 0.2f;

        [Header("Platforming")] 
        [SerializeField] private List<String> floorTags;
        
        private Rigidbody _rigidBody;
        private Transform _transform;
        private Transform _playerModelTransform;
        private Transform _cameraTransform;

        private Vector3 _direction = Vector3.forward;
        private float _speed;
        
        private bool _grounded;
        private bool _jumping;

        
        private GameManager _gameManager;
        private Health _health;
        private Transform _lastCheckpoint;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _transform = transform;
            _playerModelTransform = transform.GetChild(0).transform;
            _cameraTransform = UnityEngine.Camera.main.transform;
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _health = GetComponent<Health>();
            _health.OnDeath += () => Respawn();
            
            Physics.gravity = Vector3.down * (9.81f * gravityMultiplier);
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
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

            _speed = inputVector.magnitude * maxSpeed;
            CalculateMovementDirection(inputVector);

            if (_grounded && Input.GetButtonDown("Jump"))
            {
                _jumping = true;
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
            if (inputVector.sqrMagnitude == 0)
                return;

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
            _rigidBody.rotation = Quaternion.Lerp(_playerModelTransform.rotation, Quaternion.LookRotation(_direction),
                10f * Time.fixedDeltaTime);
            
            _rigidBody.velocity = _direction * _speed + Vector3.up * _rigidBody.velocity.y;
            
            if (_jumping)
            {
                _rigidBody.AddForce(Vector3.up * jumpForceMultiplier);
                _jumping = false;
            }
        }

        private void Respawn()
        {
            if (_lastCheckpoint == null) 
                _gameManager.RestartGame();
            else
            {
                _health.Reset();
                _transform.position = _lastCheckpoint.position + Vector3.up;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (floorTags.Contains(other.tag)) 
                _grounded = true;
            
            if (other.CompareTag("Platform"))
                _transform.parent = other.transform.parent.transform;
            
            if (other.CompareTag("Checkpoint")) 
                _lastCheckpoint = other.transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (floorTags.Contains(other.tag)) 
                _grounded = false;
            
            if (other.CompareTag("Platform") && other.transform.parent.transform == _transform.parent)
                _transform.parent = null;
        }
    }
}