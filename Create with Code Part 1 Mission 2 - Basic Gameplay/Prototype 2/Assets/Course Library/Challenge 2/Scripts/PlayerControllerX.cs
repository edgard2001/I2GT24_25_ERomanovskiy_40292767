using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    private float cooldown = 0f;

    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (cooldown <= 0 && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            cooldown = 1f;
        }
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
        }
    }
}
