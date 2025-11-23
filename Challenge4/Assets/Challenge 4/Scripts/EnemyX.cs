/*
 * David Huerta
 * Challenge 4
 * Controls enemy movement toward the player goal and handles scoring.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject playerGoal;

    // cached reference to the SpawnManager so we don't repeatedly Find
    private SpawnManagerX spawnManager;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();

        // assign playerGoal safely
        playerGoal = GameObject.Find("Player Goal");

        // cache spawn manager (make sure object is named "SpawnManager")
        GameObject smObj = GameObject.Find("SpawnManager");
        if (smObj != null)
        {
            spawnManager = smObj.GetComponent<SpawnManagerX>();
            // get initial speed from spawn manager if available
            if (spawnManager != null)
                speed = spawnManager.enemySpeed;
        }
        else
        {
            Debug.LogWarning("SpawnManager not found by EnemyX on Start()");
        }
    }

    void Update()
    {
        // If spawnManager not available, don't move
        if (spawnManager == null) return;

        // DO NOTHING until the game has started
        if (!spawnManager.gameStarted) return;

        // normal movement toward player goal once game is started
        if (playerGoal != null)
        {
            Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
            enemyRb.AddForce(lookDirection * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if the game hasn't started, ignore goal collisions
        if (spawnManager != null && !spawnManager.gameStarted) return;

        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.name == "Player Goal")
        {
            // only score / trigger loss if the game is started
            if (spawnManager != null && spawnManager.gameStarted)
            {
                Destroy(gameObject);
                spawnManager.LoseGame();
            }
        }
    }
}



