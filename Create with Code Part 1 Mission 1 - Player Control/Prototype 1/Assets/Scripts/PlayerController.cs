using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed = 20;
    private float _turnSpeed = 45;
    private float _forwardInput;
    private float _horizontalInput;

    // Update is called once per frame
    void Update()
    {
        _forwardInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        // Move vehicle forward
        transform.Translate(Vector3.forward * _speed * _forwardInput * Time.deltaTime);
        // Steer car
        transform.Rotate(Vector3.up, _turnSpeed * _horizontalInput * Time.deltaTime);
    }
}
