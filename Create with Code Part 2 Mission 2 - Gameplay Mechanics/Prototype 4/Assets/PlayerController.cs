using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5;

    private GameObject focalPoint;

    private bool hasPowerup;
    private bool hasHomingMissile = true;
    public float powerupStrength = 15;

    private GameObject powerUpIndicator;
    
    [SerializeField] private GameObject missilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        powerUpIndicator = GameObject.Find("Powerup Indicator");
        powerUpIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerUpIndicator.transform.position = transform.position;

        if (hasHomingMissile && Input.GetKeyDown(KeyCode.Space))
        {
            List<Transform> enemies = FindObjectsOfType<Enemy>().Select(enemy => enemy.transform).ToList();
            
            float minDistanceSquared = Mathf.Infinity;
            Transform closestEnemy = null;
            enemies.ForEach(t =>
            {
                float distanceSquared = (transform.position - t.position).sqrMagnitude;
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    closestEnemy = t;
                }
            });

            if (closestEnemy != null)
            {
                HomingMissile missile = Instantiate(missilePrefab, transform.position,
                    Quaternion.LookRotation(transform.position - closestEnemy.position)).GetComponent<HomingMissile>();
                missile.target = closestEnemy;
            
                //hasHomingMissile = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup")) 
        { 
            hasPowerup = true;
            powerUpIndicator.SetActive(true);
            StartCoroutine(PowerupCountdownCoroutine());
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("HomingMissilePowerUp")) 
        { 
            hasHomingMissile = true;
            powerUpIndicator.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    IEnumerator PowerupCountdownCoroutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerUpIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup) 
        {
            Vector3 awayFromPlayer = collision.transform.position - transform.position;
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}