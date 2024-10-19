using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRb;
    public float jumpForce;
    public float gravityModifier;

    private bool _isOnGround;
    public bool gameOver;

    private Animator _animator;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isOnGround = false;
            _animator.SetTrigger("Jump_trig");
            _animator.SetBool("grounded", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            _animator.SetBool("grounded", true);
        }
        else
        {
            gameOver = true; 
            Debug.Log("Game Over!");
            _animator.SetBool("Death_b", true);
        }
    }
}
