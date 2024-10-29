using System.Collections;
using System.Collections.Generic;
using Common;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, CharacterAttack
{
    [SerializeField] private Collider meleeTrigger;
    
    private Animator _animator;
    private bool _attacking;
    private bool _blocking;
    
    [SerializeField] private float staminaMax = 100;
    [SerializeField] private bool canRecoversStaminaOverTime;
    [SerializeField] private float staminaPerSecond = 10f;
    [SerializeField] private float staminaOverTimeDelayMax = 1f;
    [SerializeField] private float attackStaminaConsumption = 20;
    [SerializeField] private float blockStaminaConsumption = 30;
    
    private float _stamina;
    private float _staminaOverTimeDelay;
    [SerializeField] private RectTransform staminaBar;
    
    void Start()
    {
        meleeTrigger.enabled = false;
        _animator = GetComponent<Animator>();
        
        _stamina = staminaMax;
    }
    
    void Update()
    {
        if (_staminaOverTimeDelay > 0)
            _staminaOverTimeDelay -= Time.deltaTime;
        else if (!_blocking)
            RecoverOverTime(Time.deltaTime);
        
        if (!_attacking && Input.GetButtonDown("AttackM") && _stamina >= attackStaminaConsumption)
        {
            _animator.SetTrigger("Attack");
            _attacking = true;
            _stamina -= attackStaminaConsumption;
            UpdateStaminaBar();
            _staminaOverTimeDelay = staminaOverTimeDelayMax;
        }
        if (Input.GetButtonUp("AttackM"))
        {
            _attacking = false;
        }
        
        if (!_attacking && Input.GetAxis("AttackC") > 0.5  && _stamina >= attackStaminaConsumption)
        {
            _animator.SetTrigger("Attack");
            _attacking = true;
            _stamina -= attackStaminaConsumption;
            UpdateStaminaBar();
            _staminaOverTimeDelay = staminaOverTimeDelayMax;
        }
        if (Input.GetAxis("AttackC") < 0.5)
        {
            _attacking = false;
        }
        
        if (Input.GetButtonDown("Block"))
        {
            _animator.SetBool("Blocking", true);
            _blocking = true;
        }

        if (Input.GetButtonUp("Block"))
        {
            _animator.SetBool("Blocking", false);
            _blocking = false;
        }
    }
    
    private void RecoverOverTime(float deltaTime)
    {
        if (!canRecoversStaminaOverTime) return;
        
        float recoverAmount = staminaPerSecond * deltaTime;
        _stamina += recoverAmount;
        UpdateStaminaBar();
    }

    private void UpdateStaminaBar()
    {
        _stamina = Mathf.Clamp(_stamina, 0, staminaMax);
        staminaBar.localScale = new Vector3(_stamina / staminaMax, staminaBar.localScale.y, staminaBar.localScale.z);
    }


    public bool IsBlocking()
    {
        if (!_blocking) return _blocking;
            
        _stamina -= blockStaminaConsumption;
        if (_stamina < 0)
        {
            _animator.SetBool("Blocking", false);
            _blocking = false;
        }
        UpdateStaminaBar();
        return _blocking;
    }
    
    public bool IsAttacking()
    {
        return _attacking;
    }

    public void OnStartAttack()
    {
        meleeTrigger.enabled = true;
    }
    
    public void OnEndAttack()
    {
        meleeTrigger.enabled = false;
    }
    
}
