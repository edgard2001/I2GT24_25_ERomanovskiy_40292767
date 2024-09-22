using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinX : MonoBehaviour
{
    public float RotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
    }
}
