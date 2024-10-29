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
        
        private Transform _transform;
        private Transform _playerTransform;
        
        private bool _chasingPlayer;
        private float _followRampUpTime;
        private void Start()
        {
            _transform = GetComponent<Transform>();
            followTrigger.enabled = true;
            collectTrigger.enabled = false;
        }

        private void Update()
        {
            if (_chasingPlayer)
                _followRampUpTime -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (!_chasingPlayer) return;

            Vector3 direction = (_playerTransform.position - _transform.position).normalized;
            _transform.position += direction * ((followRampUpTimeMax - _followRampUpTime) / followRampUpTimeMax * followSpeedMax * Time.fixedDeltaTime);
        }
        
        void OnTriggerStay(Collider other)
        {
            ChaseAndCollect(other);
        }

        private void ChaseAndCollect(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (_chasingPlayer)
            {
                if (_followRampUpTime > followRampUpTimeMax - 0.5) return;
                PlayerInventory inventory = other.GetComponent<PlayerInventory>();
                inventory.AddComponent(collectibleType, amount);
                Destroy(gameObject);
            }
            else
            {
                _playerTransform = other.GetComponent<Transform>();
                _chasingPlayer = true;
                _followRampUpTime = followRampUpTimeMax;
                
                followTrigger.enabled = false;
                collectTrigger.enabled = true;
            }
        }
    }
}
