/*
 * David Huerta
 * Prototype 4
 * Handles player movement and powerup effects.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed;
    private float forwardInput;

    private GameObject focalPoint;

    public bool hasPowerup;
    private float powerupStrength = 15.0f;

    public GameObject powerupIndicator;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");
    }

    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");

        powerupIndicator.transform.position =
            transform.position + new Vector3(0, -0.5f, 0);

        
        if (transform.position.y < -5)
        {
            FindObjectOfType<SpawnManager>().LoseGame();
        }
    }

    private void FixedUpdate()
    {
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer =
                (collision.gameObject.transform.position - transform.position).normalized;

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }
}

