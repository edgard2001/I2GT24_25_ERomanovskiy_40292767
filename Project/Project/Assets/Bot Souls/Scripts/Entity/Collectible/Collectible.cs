using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectible
{
    public class Collectible : MonoBehaviour
    {
        [SerializeField] private CollectibleType collectibleType;
        [SerializeField] private int amount;
        [SerializeField] private float followSpeedMax;
        [SerializeField] private float followRampUpTimeMax = 3f;
            
        [SerializeField] private Collider followTrigger;
        [SerializeField] private Collider collectTrigger;
        [SerializeField] private Collider physicalCollider;
        
        [SerializeField] private AudioClip activateChaseAudioClip;
        
        private Transform _transform;
        private Renderer _model;
        private Transform _playerTransform;
        private Rigidbody _rigidbody;
        private AudioSource _audioSource;
        
        private bool _chasingPlayer;
        private float _followRampUpTime;

        private bool _initialised;
        private void Start()
        {
            _transform = GetComponent<Transform>();
            _model = _transform.GetChild(0).GetComponent<Renderer>();
            _rigidbody = GetComponent<Rigidbody>();
            _audioSource = GetComponent<AudioSource>();
                
            followTrigger.enabled = true;
            collectTrigger.enabled = false;
            physicalCollider.enabled = true;

            _initialised = true;
        }

        private void Update()
        {
            if (_chasingPlayer)
                _followRampUpTime -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!_chasingPlayer) return;

            Vector3 direction = _playerTransform.position + Vector3.up - _transform.position;
                
            direction.y = (followRampUpTimeMax - _followRampUpTime) / followRampUpTimeMax * direction.y;
            direction.Normalize();
            
            _rigidbody.velocity =
                direction * ((followRampUpTimeMax - _followRampUpTime) / followRampUpTimeMax * followSpeedMax) +
                Vector3.up * _followRampUpTime / followRampUpTimeMax * _rigidbody.velocity.y;
            _rigidbody.rotation = Quaternion.LookRotation(direction);
        }

        void OnTriggerStay(Collider other)
        {
            if (!_initialised) return;
            ChaseAndCollect(other);
        }

        private void ChaseAndCollect(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (_chasingPlayer)
            {
                if (_followRampUpTime > followRampUpTimeMax - 0.3) return;
                PlayerInventory inventory = other.GetComponent<PlayerInventory>();
                inventory.AddComponent(collectibleType, amount);
                _audioSource.PlayOneShot(activateChaseAudioClip);

                _model.enabled = false;
                followTrigger.enabled = false;
                collectTrigger.enabled = false;
                physicalCollider.enabled = false;
                
                Destroy(gameObject);
            }
            else
            {
                _playerTransform = other.GetComponent<Transform>();
                _chasingPlayer = true;
                _followRampUpTime = followRampUpTimeMax;
                
                _rigidbody.AddForce(Vector3.up * 20, ForceMode.Impulse);
                
                followTrigger.enabled = false;
                collectTrigger.enabled = true;
                physicalCollider.enabled = false;
                
                _audioSource.PlayOneShot(activateChaseAudioClip);
            }
        }
    }
}
