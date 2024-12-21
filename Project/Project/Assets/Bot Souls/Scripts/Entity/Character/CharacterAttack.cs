using System;
using System.Linq.Expressions;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private float attackStaminaConsumption = 20;
    [SerializeField] private float blockStaminaConsumption = 30;
    
    private MeleeWeapon[] _meleeWeapons;
    
    private Animator _animator;
    private CharacterAnimation _animationController;
    
    private Health _health;
    private Stamina _stamina;
    
    private bool _blocking;
    private bool _blockBroken;
    private bool _attackQueued;
    private bool _canQueueAttack;
    public bool Stunned;
    
    [SerializeField] private float _blockingTime;
    
    void Start()
    {
        _meleeWeapons = GetComponentsInChildren<MeleeWeapon>();
        foreach (MeleeWeapon meleeWeapon in _meleeWeapons)
            meleeWeapon.OnAttackParried += EnterStun;
        
        _animator = GetComponent<Animator>();
        _animationController = GetComponent<CharacterAnimation>();
        
        _animationController.MeleeDagameStart += StartDamage;
        _animationController.MeleeDagameEnd += EndDamage;
        _animationController.MeleeAttackEnd += EndAttack;
        
        _health = GetComponent<Health>();
        _health.Damaged += () =>
        {
            _animator.SetTrigger("Impact");
            
            EndDamage();
            _attackQueued = false;
            _animator.ResetTrigger("Attack");
            _animator.SetBool("Attacking", false);
            
            _blocking = false;
            _animator.SetBool("Blocking", false);
        };
        _health.Died += () =>
        {
            EndDamage();
            _animator.SetTrigger("Death");
        };
        
        _stamina = GetComponentInChildren<Stamina>();
    }

    private void Update()
    {
        if (_blocking)
            _blockingTime += Time.deltaTime;
    }

    public void Attack()
    {
        if (_attackQueued) return;
        if (_animator.GetBool("Attacking") && !_canQueueAttack) return;
        if (!_stamina.UseStamina(attackStaminaConsumption)) return;
        
        if (_animator.GetBool("Attacking")) _attackQueued = true;
        
        _animator.SetTrigger("Attack");
        _animator.SetBool("Blocking", false);
        _animator.SetBool("Attacking", true);
    }
    
    public void Block()
    {
        if (_blockBroken) return;
        
        if (_stamina.StaminaPercentage > 0)
            _animator.SetBool("Blocking", true);
    }
    
    public void UnBlock()
    {
        _blockBroken = false;
        _animator.SetBool("Blocking", false);
    }
    
    public bool IsBlocking()
    {
        return _blocking;
    }

    public bool AttemptBlock()
    {
        if (!_blocking) return _blocking;
            
        if (!_stamina.UseStamina(blockStaminaConsumption))
        {
            _animator.SetBool("Blocking", false);
            _blockBroken = true;
            _stamina.UseStamina(_stamina.StaminaAmount);
            EnterStun();
        }
        return true;
    }
    
    public bool IsAttacking()
    {
        return _animator.GetBool("Attacking");
    }

    private void StartDamage()
    {
        _attackQueued = false;
        _canQueueAttack = true;
        foreach (MeleeWeapon meleeWeapon in _meleeWeapons)
            meleeWeapon.StartAttack();
    }
    
    private void EndDamage()
    {
        foreach (MeleeWeapon meleeWeapon in _meleeWeapons)
            meleeWeapon.EndAttack();
    }
    
    private void EndAttack()
    {
        if (!_attackQueued) 
        {
            _animator.SetBool("Attacking", false);
        }
        _canQueueAttack = false;
    }
    
    public void StartBlock()
    {
        _blocking = true;
        _stamina.canRecoversStaminaOverTime = false;
        _blockingTime = 0;
    }
    
    public void EndBlock()
    {
        _blocking = false;
        _stamina.canRecoversStaminaOverTime = true;
    }

    public bool AttemptParry()
    {
        return _blocking && _blockingTime < _meleeWeapons[0].ParryWindow;
    }

    private void EnterStun()
    {
        EndDamage();
        _attackQueued = false;
        _animator.ResetTrigger("Attack");
        _animator.SetBool("Attacking", false);
            
        _blocking = false;
        _animator.SetBool("Blocking", false);
            
        _animator.SetTrigger("Stun");
    }
}
