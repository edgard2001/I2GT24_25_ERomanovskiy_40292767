using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;

    private float minSpeed = 10;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -1;

    private GameManager gameManager;
    public int pointValue;

    public ParticleSystem explosionEffect;

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(
            RandomForce(),
            ForceMode.Impulse
        );
        targetRb.AddTorque(
            RandomTorque(),
            RandomTorque(),
            RandomTorque(),
            ForceMode.Impulse
        );
        transform.position = RandomSpawnPos();
    }

    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnMouseDown()
    {
        if (!gameManager.isActive) return;

        Destroy(gameObject);
        gameManager.UpdateScore(pointValue);

        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);

        if (CompareTag("Bad")) gameManager.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        if (!other.CompareTag("Bad")) gameManager.GameOver();
    }

}
