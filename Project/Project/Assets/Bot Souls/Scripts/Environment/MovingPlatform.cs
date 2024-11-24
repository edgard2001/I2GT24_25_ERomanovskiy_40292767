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
    
        // Start is called before the first frame update
        void Start()
        {
            _transform = transform;
            _startPosition = _transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _transform.position = _startPosition + (Mathf.Sin(Time.realtimeSinceStartup / 2 * oscillationRate) + 1) * movementRange * movementDirection;
            _transform.rotation = Quaternion.AngleAxis(rotationSpeed * Time.fixedDeltaTime, rotationAxis) * _transform.rotation;
        }
    }

}