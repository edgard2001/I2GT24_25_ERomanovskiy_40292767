using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField] private float staminaMax = 100;
    public bool canRecoversStaminaOverTime;
    
    [SerializeField] private float staminaPerSecond = 10f;
    [SerializeField] private float staminaOverTimeDelayMax = 1f;
    
    private float _stamina;
    public float StaminaPercentage => _stamina / staminaMax;
    public float StaminaAmount => _stamina;
    
    private float _staminaOverTimeDelay;
    
    [SerializeField] private RectTransform staminaBar;
    [SerializeField] private RectTransform consumptionIndicatorBar;
    
    [SerializeField] private float consumptionIndicatorCatchupTimeMax = 2;
    private float _consumptionIndicatorCatchupTime;
    private Vector3 _previousConsumptionIndicatorScale;
    
    private void Start()
    {
        _stamina = staminaMax;
        UpdateStaminaBar();
    }
    
    void Update()
    {
        if (_staminaOverTimeDelay > 0)
            _staminaOverTimeDelay -= Time.deltaTime;
        else
            RecoverOverTime(Time.deltaTime);
        
        if (_consumptionIndicatorCatchupTime < consumptionIndicatorCatchupTimeMax)
            _consumptionIndicatorCatchupTime += Time.deltaTime;

        if (consumptionIndicatorBar != null && staminaBar != null)
            consumptionIndicatorBar.localScale = Vector3.Lerp(_previousConsumptionIndicatorScale, staminaBar.localScale,
                _consumptionIndicatorCatchupTime / consumptionIndicatorCatchupTimeMax);
    }
    
    private void RecoverOverTime(float deltaTime)
    {
        if (!canRecoversStaminaOverTime) return;
        
        float recoverAmount = staminaPerSecond * deltaTime;
        _stamina += recoverAmount;
        UpdateStaminaBar();
    }

    public bool UseStamina(float amount)
    {
        if (amount <= 0) return false;
        if (_stamina < amount) return false;
        
        _staminaOverTimeDelay = staminaOverTimeDelayMax;
        
        _stamina -= amount;
        UpdateStaminaBar();
        _consumptionIndicatorCatchupTime = 0;
        _previousConsumptionIndicatorScale = consumptionIndicatorBar.localScale;

        return true;
    }
    
    private void UpdateStaminaBar()
    {
        _stamina = Mathf.Clamp(_stamina, 0, staminaMax);
        staminaBar.localScale = new Vector3(_stamina / staminaMax, staminaBar.localScale.y, staminaBar.localScale.z);
    }
}
