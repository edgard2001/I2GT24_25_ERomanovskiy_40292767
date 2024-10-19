using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    public float jumpForce;
    public float gravityModifier;

    private bool _isOnGround;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isOnGround = true;
    }
}
