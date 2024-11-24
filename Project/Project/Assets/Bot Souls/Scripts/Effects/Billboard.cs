using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _transform;
    private Transform _cameraTransform;
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        _cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        _transform.rotation = Quaternion.LookRotation(_cameraTransform.forward);
    }
}
