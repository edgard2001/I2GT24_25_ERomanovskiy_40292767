using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10.0f;
    public float LimitX = 20.0f;
    public GameObject Food;

    private float _horizontalInput;

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * _horizontalInput * Time.deltaTime * Speed);

        if (Mathf.Abs(transform.position.x) > LimitX)
            transform.position -= (transform.position.x - Mathf.Sign(transform.position.x) * LimitX) * transform.right;

        if (Input.GetKeyDown(KeyCode.Space))
            Instantiate(Food, transform.position, Food.transform.rotation);
    }
}
