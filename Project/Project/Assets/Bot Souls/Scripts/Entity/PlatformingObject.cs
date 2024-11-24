using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingObject : MonoBehaviour
{
    public event Action OnTouchGround;
    public event Action OnLeaveGround;
    
    [Header("Platforming")] 
    [SerializeField] private List<String> floorTags;
    
    [SerializeField] private Transform objectTransform;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Platform"))
            objectTransform.parent = other.transform.parent.transform;
            
        if (floorTags.Contains(other.tag))
        {
            OnTouchGround?.Invoke();
        }
    }
        
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Platform") && objectTransform.parent == null)
            objectTransform.parent = other.transform.parent.transform;
            
        if (floorTags.Contains(other.tag))
        {
            OnTouchGround?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Platform") && other.transform.parent.transform == objectTransform.parent)
            objectTransform.parent = null;
            
        if (floorTags.Contains(other.tag) && objectTransform.parent == null)
        {
            OnLeaveGround?.Invoke();
        }
    }
}
