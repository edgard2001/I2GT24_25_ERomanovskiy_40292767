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
    public ParticleSystem explosion;
    public ParticleSystem trail;

    public AudioClip jumpSound;
    public AudioClip crashSound;
    private AudioSource _audioSource;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        
        _animator = GetComponent<Animator>();
        trail.Play();
        
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_isOnGround && Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isOnGround = false;
            _animator.SetTrigger("Jump_trig");
            _animator.SetBool("grounded", false);
            trail.Stop();
            _audioSource.PlayOneShot(jumpSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isOnGround = true;
            _animator.SetBool("grounded", true);
            trail.Play();
        }
        else
        {
            gameOver = true; 
            Debug.Log("Game Over!");
            _animator.SetBool("Death_b", true);
            explosion.Play();
            trail.Stop();
            _audioSource.PlayOneShot(crashSound, 1.0f);
        }
    }
}
