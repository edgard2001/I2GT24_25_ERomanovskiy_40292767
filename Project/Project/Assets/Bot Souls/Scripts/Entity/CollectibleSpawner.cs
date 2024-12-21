using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] hitComponentPrefabs;
    [SerializeField] private GameObject[] deathComponentPrefabs;
    [SerializeField] private float ejectForce = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        Health health = GetComponent<Health>();
        health.Died += () => SpawnComponents(deathComponentPrefabs, Random.Range(4,7));
        health.Damaged += () => SpawnComponents(hitComponentPrefabs, 1);
    }

    // Update is called once per frame
    void SpawnComponents(GameObject[] componentPrefabs, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject component = Instantiate(componentPrefabs[Random.Range(0, componentPrefabs.Length)], transform.position, Quaternion.identity);
            Vector2 offset2 = Random.insideUnitCircle * 0.3f;
            Vector3 offset3 = new Vector3(offset2.x, 0, offset2.y);
            Vector3 direction = (Vector3.up + offset3).normalized;
            Debug.DrawLine(transform.position, transform.position + direction, Color.red, 1);
            component.GetComponent<Rigidbody>().AddForce(direction * ejectForce, ForceMode.Impulse);
        }
    }
}
