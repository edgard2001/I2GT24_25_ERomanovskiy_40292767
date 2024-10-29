using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Collectible;
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private TMP_Text[] componentCountUI = new TMP_Text[(int)CollectibleType.CollectibleTypeCount];
    private readonly int[] _componentCounts = new int[(int)CollectibleType.CollectibleTypeCount];

    public void AddComponent(Collectible.CollectibleType type, int amount)
    {
        _componentCounts[(int)type] += amount;
        componentCountUI[(int)type].text = $"{_componentCounts[(int)type]}";
    }
}