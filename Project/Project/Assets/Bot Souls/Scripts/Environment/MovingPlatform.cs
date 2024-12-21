using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Platform
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 movementDirection = Vector3.up;
        [SerializeField] private float oscillationRate = 1;
        [SerializeField] private float movementRange = 5;
        
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        [SerializeField] private float rotationSpeed = 40;
        
        private Transform _transform;
        private Vector3 _startPosition;
        [SerializeField]  private bool enabled;
        private float _time;
        
        public event Action<bool> OnStatusChanged; 
        
        void Start()
        {
            _transform = transform;
            _startPosition = _transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!enabled) return;
            
            _transform.position = _startPosition + (-Mathf.Cos(_time * 2 * Mathf.PI * oscillationRate) + 1) / 2 * movementRange * movementDirection;
            _transform.rotation = Quaternion.AngleAxis(rotationSpeed * Time.fixedDeltaTime, rotationAxis) * _transform.rotation;
            
            _time += Time.fixedDeltaTime;
            float oscillationPeriod = 1 / oscillationRate;
            if (_time >= oscillationPeriod) _time -= oscillationPeriod;
        }

        public void Toggle()
        {
            enabled = !enabled;
            OnStatusChanged?.Invoke(enabled);
        }
    }

}