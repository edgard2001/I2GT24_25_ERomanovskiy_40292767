using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Health health = other.GetComponentInChildren<Health>();
        if (health)
            health.TakeDamage(9999);
    }
}
