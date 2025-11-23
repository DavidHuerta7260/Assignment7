/*
 * David Huerta
 * Prototype 4
 * Controls spawning, waves, and win/loss logic.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    public Text waveText;       
    public Text messageText;    

    private float spawnRange = 9;

    public int enemyCount;
    public int waveNumber = 1;

    public bool gameStarted = false;

    void Start()
    {
        
        messageText.text = "Press SPACE to Start";
        messageText.gameObject.SetActive(true);
    }

    void Update()
    {
        
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameStarted = true;
                messageText.gameObject.SetActive(false);

                SpawnEnemyWave(waveNumber);
                SpawnPowerup(1);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        
        waveText.text = "Wave: " + waveNumber;

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            waveNumber++;

            if (waveNumber > 10)
            {
                WinGame();
                return;
            }

            SpawnEnemyWave(waveNumber);
            SpawnPowerup(1);
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerup(int powerupsToSpawn)
    {
        for (int i = 0; i < powerupsToSpawn; i++)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(spawnPosX, 0, spawnPosZ);
    }

    public void WinGame()
    {
        gameStarted = false;
        messageText.text = "YOU WIN! Press R to Restart";
        messageText.gameObject.SetActive(true);
    }

    public void LoseGame()
    {
        gameStarted = false;
        messageText.text = "YOU LOSE! Press R to Restart";
        messageText.gameObject.SetActive(true);
    }
}

