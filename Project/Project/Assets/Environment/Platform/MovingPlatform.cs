using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Platform
{
    public class MovingPlatform : MonoBehaviour
    {
        private Rigidbody _rigidbody;
    
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _rigidbody.velocity = Mathf.Sin(Time.realtimeSinceStartup) * 5 * Vector3.up;
            //_transform.rotation = Quaternion.AngleAxis(40 * Time.deltaTime, Vector3.up) * _transform.rotation;
        }
    }

}