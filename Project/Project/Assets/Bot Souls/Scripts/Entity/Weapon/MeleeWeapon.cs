using System;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private string tagToDamage;
    [SerializeField] private float damage = 10;
    [SerializeField] private float parryWindow;
    public float ParryWindow => parryWindow;
    
    [SerializeField] private GameObject impactParticleEffect;
    [SerializeField] private GameObject blockParticleEffect;

    [SerializeField] private AudioClip impactSoundEffect;
    [SerializeField] private AudioClip blockSoundEffect;
    [SerializeField] private AudioClip parrySoundEffect;
    
    [SerializeField] private LayerMask mask;

    public event Action OnAttackParried;
    
    private AudioSource _audioSource;
    private Collider _collider;
    private Transform _colliderTransform;

    private bool _attacking;
    private Vector3 _lastPosition;
    private Vector3 _lastDirection;
    
    private List<Transform> _transformsHit;
    [SerializeField] private int maxEnemiesHitCount = 3;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _collider = GetComponentInChildren<Collider>();
        _collider.enabled = false;
            
        _colliderTransform = _collider.transform;
        _transformsHit = new List<Transform>(3);
    }

    public void StartAttack()
    {
        _transformsHit.Clear();
        _attacking = true;
    }

    public void EndAttack()
    {
        _attacking = false;
    }

    private void FixedUpdate()
    {
        if (!_attacking) return;
        
        TraceHit(0.15f, 1.25f);
        _lastPosition = _collider.bounds.center;
        _lastDirection = _colliderTransform.up;
    }

    private void TraceHit(float offset, float length)
    {
        Vector3 origin1 = _collider.bounds.center - _colliderTransform.up * offset;
        Vector3 origin2 = _collider.bounds.center - _colliderTransform.up * offset - _colliderTransform.forward * 0.2f;
        Vector3 origin3 = _collider.bounds.center - _colliderTransform.up * offset - _colliderTransform.forward * 0.1f;

        RaycastHit[] hits1 = Physics.RaycastAll(origin1, _colliderTransform.up, length, mask);
        RaycastHit[] hits2 = Physics.RaycastAll(origin2, _colliderTransform.up, length, mask);
        RaycastHit[] hits3 = Physics.RaycastAll(origin3, _colliderTransform.up, length, mask);

        Debug.DrawLine(origin1, origin1 + _colliderTransform.up * length, Color.red, 0.1f);
        Debug.DrawLine(origin2, origin2 + _colliderTransform.up * length, Color.red, 0.1f);
        Debug.DrawLine(origin3, origin3 + _colliderTransform.up * length, Color.red, 0.1f);
        
        
        
        if (hits1.Length + hits2.Length + hits3.Length == 0) return;

        RaycastHit[] hits = hits3;
        if (hits2.Length > 0) hits = hits2;
        if (hits1.Length > 0) hits = hits1;
        
        foreach (RaycastHit hit in hits)
        {
            if (_transformsHit.Contains(hit.transform) || _transformsHit.Count == maxEnemiesHitCount) return;
            _transformsHit.Add(hit.transform);
            
            Debug.DrawLine(origin2, hit.point, Color.green, 1);
            Health health = hit.transform.GetComponentInChildren<Health>();

            if (health != null)
            {
                if (hit.transform.CompareTag(tagToDamage))
                {
                    DoDamage(hit.transform, health, hit);
                }
                else
                {
                    health.TakeDamage(damage);
                    HitCharacter(hit);
                }
            }
            else 
            {
                HitBlock(hit);
            }
        }
    }

    private void DoDamage(Transform other, Health health, RaycastHit hit)
    {
        CharacterAttack characterAttack = other.GetComponentInChildren<CharacterAttack>();
        if (characterAttack.AttemptParry())
        {
            OnAttackParried?.Invoke();
            _audioSource.PlayOneShot(parrySoundEffect);
            HitBlock(hit);
        }
        else if (!characterAttack.AttemptBlock())
        {
            health.TakeDamage(damage);
            HitCharacter(hit);
        }
        else
        {
            HitBlock(hit);
        }
    }
    
    private void HitCharacter(RaycastHit hit)
    {
        _audioSource.PlayOneShot(impactSoundEffect);
        Instantiate(impactParticleEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }

    private void HitBlock(RaycastHit hit)
    {
        _audioSource.PlayOneShot(blockSoundEffect);
        Instantiate(blockParticleEffect, hit.point, Quaternion.LookRotation(hit.normal));
    }
}