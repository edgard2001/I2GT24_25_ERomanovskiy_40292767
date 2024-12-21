using System;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MeleeWeapon
{
    protected override void TraceHit()
    {
        float offset = 1.5f;
        float length = 1.5f;
        
        Vector3 origin1 = _collider.bounds.center - _colliderTransform.forward * offset;
        Vector3 origin2 = _collider.bounds.center - _colliderTransform.forward * offset - _colliderTransform.up * 0.2f;
        Vector3 origin3 = _collider.bounds.center - _colliderTransform.forward * offset - _colliderTransform.up * 0.1f;
        
        Vector3 origin4 = origin1 + _colliderTransform.right * 0.1f;
        Vector3 origin5 = origin2 + _colliderTransform.right * 0.1f;
        Vector3 origin6 = origin3 + _colliderTransform.right * 0.1f;
        
        Vector3 origin7 = origin1 - _colliderTransform.right * 0.1f;
        Vector3 origin8 = origin2 - _colliderTransform.right * 0.1f;
        Vector3 origin9 = origin3 - _colliderTransform.right * 0.1f;

        RaycastHit[] hits1 = Physics.RaycastAll(origin1, _colliderTransform.forward, length, mask);
        RaycastHit[] hits2 = Physics.RaycastAll(origin2, _colliderTransform.forward, length, mask);
        RaycastHit[] hits3 = Physics.RaycastAll(origin3, _colliderTransform.forward, length, mask);
        RaycastHit[] hits4 = Physics.RaycastAll(origin4, _colliderTransform.forward, length, mask);
        RaycastHit[] hits5 = Physics.RaycastAll(origin5, _colliderTransform.forward, length, mask);
        RaycastHit[] hits6 = Physics.RaycastAll(origin6, _colliderTransform.forward, length, mask);
        RaycastHit[] hits7 = Physics.RaycastAll(origin7, _colliderTransform.forward, length, mask);
        RaycastHit[] hits8 = Physics.RaycastAll(origin8, _colliderTransform.forward, length, mask);
        RaycastHit[] hits9 = Physics.RaycastAll(origin9, _colliderTransform.forward, length, mask);

        Debug.DrawLine(origin1, origin1 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin2, origin2 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin3, origin3 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin4, origin4 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin5, origin5 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin6, origin6 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin7, origin7 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin8, origin8 + _colliderTransform.forward * length, Color.red, 0.1f);
        Debug.DrawLine(origin9, origin9 + _colliderTransform.forward * length, Color.red, 0.1f);
        
        if (hits1.Length + hits2.Length + hits3.Length + 
            hits4.Length + hits5.Length + hits6.Length + 
            hits7.Length + hits8.Length + hits9.Length == 0) 
            return;

        RaycastHit[] hits = hits9;
        if (hits6.Length > 0) hits = hits6;
        if (hits3.Length > 0) hits = hits3;
        
        if (hits8.Length > 0) hits = hits8;
        if (hits5.Length > 0) hits = hits5;
        if (hits2.Length > 0) hits = hits2;
        
        if (hits7.Length > 0) hits = hits7;
        if (hits4.Length > 0) hits = hits4;
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
}