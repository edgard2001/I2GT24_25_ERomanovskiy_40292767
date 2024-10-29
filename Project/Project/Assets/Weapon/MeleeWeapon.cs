using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private string tagToDamage;

    [SerializeField] private GameObject impactParticleEffect;
    [SerializeField] private GameObject blockParticleEffect;

    [SerializeField] private AudioClip impactSoundEffect;
    [SerializeField] private AudioClip blockSoundEffect;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToDamage))
        {
            DoDamage(other);
        }
    }

    private void DoDamage(Collider other)
    {
        Health health = other.GetComponent<Health>();
        Transform otherTransform = other.transform;

        if (!other.GetComponent<CharacterAttack>().IsBlocking())
        {
            health.TakeDamage(damage);
            _audioSource.PlayOneShot(impactSoundEffect);
            Instantiate(impactParticleEffect, otherTransform.position, otherTransform.rotation);
        }
        else
        {
            _audioSource.PlayOneShot(blockSoundEffect);
            Instantiate(blockParticleEffect, otherTransform.position, otherTransform.rotation);
        }
    }
}