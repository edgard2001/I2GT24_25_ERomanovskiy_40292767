using System;
using System.Collections;
using System.Collections.Generic;
using Collectible;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradesMenu : MonoBehaviour
{
    public event Action<int> OnHealthLevelChanged;
    public event Action<int> OnStaminaLevelChanged;
    public event Action<int> OnDamageLevelChanged;
    
    [SerializeField] private TMP_Text healthLevelText;
    [SerializeField] private TMP_Text staminaLevelText;
    [SerializeField] private TMP_Text damageLevelText;
    
    private int _healthLevel;
    private int _staminaLevel;
    private int _damageLevel;
    
    [SerializeField] private TMP_Text healthCostText;
    [SerializeField] private TMP_Text staminaCostText;
    [SerializeField] private TMP_Text damageCostText;
    
    private int _healthCost = 50;
    private int _staminaCost = 50;
    private int _damageCost = 50;

    [SerializeField] private int healthLevelMax = 100;
    [SerializeField] private int staminaLevelMax = 100;
    [SerializeField] private int damageLevelMax = 100;
    
    [SerializeField] private AnimationCurve healthCostCurve;
    [SerializeField] private AnimationCurve staminaCostCurve;
    [SerializeField] private AnimationCurve damageCostCurve;

    private bool _inMenu;
    public bool InMenu => _inMenu;
    [SerializeField] private GameObject upgradesUI;
    [SerializeField] private PlayerInventory playerInventory;
    
    [SerializeField] private GameObject firstButton;
    
    private void Start()
    {
        healthLevelText.text = $"Health Level: {_healthLevel}";
        staminaLevelText.text = $"Stamina Level: {_staminaLevel}";
        damageLevelText.text = $"Damage Level: {_damageLevel}";
        
        healthCostText.text = $"Upgrade Cost: {_healthCost}";
        staminaCostText.text = $"Upgrade Cost: {_staminaCost}";
        damageCostText.text = $"Upgrade Cost: {_damageCost}";
        
        OnHealthLevelChanged?.Invoke(_healthLevel);
        OnStaminaLevelChanged?.Invoke(_staminaLevel);
        OnDamageLevelChanged?.Invoke(_damageLevel);
        
        upgradesUI.SetActive(_inMenu);
        Cursor.visible = _inMenu;
        Cursor.lockState = _inMenu ? CursorLockMode.Confined : CursorLockMode.Locked; 
    }
    
    private void Update()
    {
        if (Input.GetButtonDown("Upgrades"))
        {
            EventSystem.current.SetSelectedGameObject(firstButton);
            ToggleUpgradesMenu();
        }
    }
    
    private void ToggleUpgradesMenu()
    {
        _inMenu = !_inMenu;
        upgradesUI.SetActive(_inMenu);
        Cursor.visible = _inMenu;
        Cursor.lockState = _inMenu ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    public void UpgradeHealth()
    {
        if (_healthLevel >= healthLevelMax) return;
        if (!playerInventory.AddComponent(CollectibleType.Common, -_healthCost)) return;
        
        _healthLevel++;
        _healthCost = (int)(_healthCost * (1 + healthCostCurve.Evaluate((float)_healthLevel / healthLevelMax)));
        healthLevelText.text = $"Health Level: {_healthLevel}";
        healthCostText.text = $"Upgrade Cost: {_healthCost}";
        OnHealthLevelChanged?.Invoke(_healthLevel);
    }
    
    public void UpgradeStamina()
    {
        if (_staminaLevel >= staminaLevelMax) return;
        if (!playerInventory.AddComponent(CollectibleType.Common, -_staminaCost)) return;
        
        _staminaLevel++;
        _staminaCost = (int)(_staminaCost * (1 + staminaCostCurve.Evaluate((float)_staminaLevel / staminaLevelMax)));
        staminaLevelText.text = $"Stamina Level: {_staminaLevel}";
        staminaCostText.text = $"Upgrade Cost: {_staminaCost}";
        OnStaminaLevelChanged?.Invoke(_staminaLevel);
    }
    
    public void UpgradeDamage()
    {
        if (_damageLevel >= damageLevelMax) return;
        if (!playerInventory.AddComponent(CollectibleType.Common, -_damageCost)) return;
        
        _damageLevel++;
        _damageCost = (int)(_damageCost * (1 + damageCostCurve.Evaluate((float)_damageLevel / damageLevelMax)));
        damageLevelText.text = $"Damage Level: {_damageLevel}";
        damageCostText.text = $"Upgrade Cost: {_damageCost}";
        OnDamageLevelChanged?.Invoke(_damageLevel);
    }
}
