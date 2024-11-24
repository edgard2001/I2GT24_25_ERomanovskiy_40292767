using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterEffect : MonoBehaviour
{
    void Start()
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        float duration = particle.main.duration + particle.main.startLifetime.Evaluate(0);
        Invoke(nameof(DestroySelf), duration);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
