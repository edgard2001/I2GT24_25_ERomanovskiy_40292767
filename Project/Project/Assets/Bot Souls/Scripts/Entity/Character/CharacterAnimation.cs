using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public event Action MeleeDagameStart;
    public event Action  MeleeDagameEnd;
    public event Action MeleeAttackEnd;
    
    public void OnStartMeleeDamage()
    {
        MeleeDagameStart?.Invoke();
    }
    
    public void OnEndMeleeDamage()
    {
        MeleeDagameEnd?.Invoke();
    }
    
    public void OnEndMeleeAttack()
    {
        MeleeAttackEnd?.Invoke();
    }
}
