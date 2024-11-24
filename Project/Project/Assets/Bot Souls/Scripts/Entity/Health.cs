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
    [SerializeField] private float stunnedDamageMultiplier = 2f;

    public event Action Damaged;
    public event Action Died;
    
    private float _health;
    private float _healOverTimeDelay;
    
    private CharacterAttack _attackBehaviour;
    
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform damageIndicatorBar;
    [SerializeField] private RectTransform entityUI;

    [SerializeField] private float damageIndicatorCatchupTimeMax = 2;
    private float _damageIndicatorCatchupTime;
    private Vector3 _previousDamageIndicatorScale;
    
    private void Start()
    {
        _attackBehaviour = GetComponent<CharacterAttack>();
        
        _health = healthMax;
        UpdateHealthBar();
    }

    private void Update()
    {
        if (_healOverTimeDelay > 0)
            _healOverTimeDelay -= Time.deltaTime;
        else
            HealOverTime(Time.deltaTime);

        if (_damageIndicatorCatchupTime < damageIndicatorCatchupTimeMax)
            _damageIndicatorCatchupTime += Time.deltaTime;

        if (damageIndicatorBar != null && healthBar != null)
            damageIndicatorBar.localScale = Vector3.Lerp(_previousDamageIndicatorScale, healthBar.localScale,
                _damageIndicatorCatchupTime / damageIndicatorCatchupTimeMax);
    }

    private void HealOverTime(float deltaTime)
    {
        if (!canHealOverTime) return;
        
        float healAmount = healPerSecond * deltaTime;
        _health += healAmount;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        if (_health <= 0) return;
        
        Damaged();
        
        _healOverTimeDelay = healOverTimeDelayMax;
        
        if (_attackBehaviour != null && _attackBehaviour.Stunned) damage *= stunnedDamageMultiplier;
        _health -= damage;
        UpdateHealthBar();
        _damageIndicatorCatchupTime = 0;
        if (damageIndicatorBar != null) _previousDamageIndicatorScale = damageIndicatorBar.localScale;

        if (_health == 0)
        {
            Died();
            Hide();
        }
    }
    
    private void Hide()
    {
        if (entityUI != null)
            entityUI.gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        _health = Mathf.Clamp(_health, 0, healthMax);
        if (healthBar != null) healthBar.localScale = new Vector3(_health / healthMax, healthBar.localScale.y, healthBar.localScale.z);
    }
}
