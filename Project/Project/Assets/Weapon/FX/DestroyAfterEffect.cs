using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{

    [SerializeField] private ParticleSystem _particleSystem;
    
    void Start()
    {
        float duration = _particleSystem.duration + _particleSystem.startLifetime;
        Invoke(nameof(DestroySelf), duration);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
