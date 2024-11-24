using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Collectible;
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private TMP_Text[] componentCountUI = new TMP_Text[(int)CollectibleType.CollectibleTypeCount];
    [SerializeField] private float componentCountCatchUpTimeMax = 1f;
    
    private readonly int[] _componentCounts = new int[(int)CollectibleType.CollectibleTypeCount];
    private readonly int[] _previousComponentCounts = new int[(int)CollectibleType.CollectibleTypeCount];
    private readonly int[] _targetComponentCounts = new int[(int)CollectibleType.CollectibleTypeCount];
    private readonly float[] _componentCountCatchUpTimes = new float[(int)CollectibleType.CollectibleTypeCount];

    private void Update()
    {
        for (int i = 0; i < _componentCounts.Length; i++)
        {
            if (_componentCounts[i] == _targetComponentCounts[i]) continue;
            if (_componentCountCatchUpTimes[i] < componentCountCatchUpTimeMax)
                _componentCountCatchUpTimes[i] += Time.deltaTime;

            _componentCounts[i] = (int)Mathf.Lerp(_previousComponentCounts[i], _targetComponentCounts[i],
                    _componentCountCatchUpTimes[i] / componentCountCatchUpTimeMax);
                componentCountUI[i].text = $"{_componentCounts[i]}";
        }
    }

    public void AddComponent(CollectibleType type, int amount)
    {
        if (Mathf.Abs(amount) > 1)
        {
            _previousComponentCounts[(int)type] = _componentCounts[(int)type];
            _componentCountCatchUpTimes[(int)type] = 0;
        } 
        else 
        {
            _componentCounts[(int)type] += amount;
            componentCountUI[(int)type].text = $"{_componentCounts[(int)type]}";
        }
        
        _targetComponentCounts[(int)type] += amount;
    }
}