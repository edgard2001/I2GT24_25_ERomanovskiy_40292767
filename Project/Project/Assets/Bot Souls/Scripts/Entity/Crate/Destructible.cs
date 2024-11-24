using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject pieces;
    
    [SerializeField] private float despawnDelay = 2;
    // Start is called before the first frame update
    void Start()
    {
        model.SetActive(true);
        pieces.SetActive(false);
        
        GetComponent<Health>().Died += () =>
        {
            model.SetActive(false);
            pieces.SetActive(true);
            Invoke(nameof(Disappear), despawnDelay);
        };
    }

    // Update is called once per frame
    void Disappear()
    {
        Destroy(gameObject);
    }
}
