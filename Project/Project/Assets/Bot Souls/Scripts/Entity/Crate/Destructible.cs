using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject[] pieces;
    
    [SerializeField] private float despawnDelay = 2;
    // Start is called before the first frame update
    void Start()
    {
        model.SetActive(true);
        foreach (GameObject piece in pieces)
            piece.SetActive(false);

        GetComponent<Health>().Died += () =>
        {
            model.SetActive(false);
            foreach (GameObject piece in pieces)
                piece.SetActive(true);
            Invoke(nameof(Disappear), despawnDelay);
        };
    }

    // Update is called once per frame
    void Disappear()
    {
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }
        Destroy(gameObject);
    }
}
