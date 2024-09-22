using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 20;

    // Update is called once per frame
    void Update()
    {
        // Move vehicle forward
        transform.Translate(transform.forward * Time.deltaTime * Speed);
    }
}
