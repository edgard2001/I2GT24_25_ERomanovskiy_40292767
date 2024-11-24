using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleJiggle : MonoBehaviour
{
    [SerializeField] private float waitPeriod = 2;
    [SerializeField] private float jumpForceMultiplier = 2;
    private float _waitTime;
    
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _waitTime = Random.Range(0, waitPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime <= 0)
        {
            _rigidbody.AddForce(Vector3.up * jumpForceMultiplier, ForceMode.Impulse);
            _waitTime = waitPeriod;
        }
    }
}
