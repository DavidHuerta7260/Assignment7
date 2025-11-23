/*
 * David Huerta
 * Challenge 4
 * Manages waves, spawning enemies/powerups, UI, game start, win/loss, and resets.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    public Text waveText;
    public Text messageText;

    private float spawnRangeX = 10;
    private float spawnZMin = 15;
    private float spawnZMax = 25;

    public int enemyCount;
    public int waveCount = 1;

    public GameObject player;

    public float enemySpeed = 10f;

    public bool gameStarted = false;

    void Start()
    {
        messageText.text = "Press SPACE to Start";
        messageText.gameObject.SetActive(true);
        waveText.text = "";
    }

    void Update()
    {
        // BEFORE GAME STARTS
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        // GAME STARTED
        waveText.text = "Wave: " + waveCount;

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            waveCount++;

            if (waveCount > 10)
            {
                WinGame();
                return;
            }

            SpawnEnemyWave(waveCount);
        }

        // R to restart after win/loss
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void StartGame()
    {
        Debug.Log("Game Started!");

        gameStarted = true;
        player.GetComponent<PlayerControllerX>().canMove = true;

        messageText.gameObject.SetActive(false);

        waveText.text = "Wave: " + waveCount;

        SpawnEnemyWave(waveCount);
    }

    Vector3 GenerateSpawnPosition()
    {
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float zPos = Random.Range(spawnZMin, spawnZMax);
        return new Vector3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int number)
    {
        // Spawn powerup if none in scene
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
        {
            Vector3 offset = new Vector3(0, 0, -15);
            Instantiate(powerupPrefab, GenerateSpawnPosition() + offset, powerupPrefab.transform.rotation);
        }

        // Spawn N enemies
        for (int i = 0; i < number; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }

        enemySpeed += 2f;  // make harder each wave

        // Reset player between waves
        ResetPlayerPosition();
    }

    void ResetPlayerPosition()
    {
        player.transform.position = new Vector3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    public void LoseGame()
    {
        messageText.text = "You Lose! Press R to Restart";
        messageText.gameObject.SetActive(true);
        gameStarted = false;
        player.GetComponent<PlayerControllerX>().canMove = false;
    }

    public void WinGame()
    {
        messageText.text = "You Win! Press R to Restart";
        messageText.gameObject.SetActive(true);
        gameStarted = false;
        player.GetComponent<PlayerControllerX>().canMove = false;
    }
}


