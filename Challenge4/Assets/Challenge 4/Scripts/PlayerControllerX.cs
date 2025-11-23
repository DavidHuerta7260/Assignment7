/*
 * David Huerta
 * Challenge 4
 * Handles player movement, turbo boost, powerups, and collision interactions.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;

    public Transform focalPoint;          // Assigned in Inspector — NO MORE NULL
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10;
    private float powerupStrength = 25;

    public bool canMove = false;          // Movement disabled until game starts

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        focalPoint.rotation = Camera.main.transform.parent.rotation;

        // Block movement until SpawnManager enables it
        if (!canMove) return;

        float verticalInput = Input.GetAxis("Vertical");

        // Always move in a flat, horizontal direction
        Vector3 moveDir = focalPoint.forward;
        moveDir.y = 0;
        moveDir.Normalize();

        playerRb.AddForce(moveDir * verticalInput * speed * Time.deltaTime);


        // Turbo boost (flat only)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 boostDir = focalPoint.forward;
        }


        // Position powerup indicator under the player
        powerupIndicator.transform.position =
            transform.position + new Vector3(0, -0.6f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromPlayer =
                (other.transform.position - transform.position).normalized;

            if (hasPowerup)
            {
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else
            {
                enemyRb.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}

