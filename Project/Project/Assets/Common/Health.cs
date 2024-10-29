using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float healthMax = 100;
    [SerializeField] private bool canHealOverTime;
    [SerializeField] private float healPerSecond = 10f;
    [SerializeField] private float healOverTimeDelayMax = 20f;

    public event Action OnDeath;
    
    private float _health;
    private float _healOverTimeDelay;
    
    [SerializeField] private RectTransform healthBar;

    private void Start()
    {
        Reset();
    }
    
    public void Reset()
    {
        _health = healthMax;
        _healOverTimeDelay = 0;
        healthBar.localScale = new Vector3(_health / healthMax, healthBar.localScale.y, healthBar.localScale.z);
    }

    private void Update()
    {
        if (_healOverTimeDelay > 0)
            _healOverTimeDelay -= Time.deltaTime;
        else
            HealOverTime(Time.deltaTime);
    }

    private void HealOverTime(float deltaTime)
    {
        if (!canHealOverTime) return;
        
        float healAmount = healPerSecond * deltaTime;
        _health = Mathf.Clamp(_health + healAmount, 0, healthMax);
        healthBar.localScale = new Vector3(_health / healthMax, healthBar.localScale.y, healthBar.localScale.z);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        
        _healOverTimeDelay = healOverTimeDelayMax;
        
        _health = Mathf.Clamp(_health - damage, 0, healthMax);
        healthBar.localScale = new Vector3(_health / healthMax, healthBar.localScale.y, healthBar.localScale.z);
        
        if (_health == 0) OnDeath();
    }
}
