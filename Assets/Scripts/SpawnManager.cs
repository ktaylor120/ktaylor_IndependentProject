using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject speedUpPrefab;
    public GameObject enemyPrefab;
    private float spawnRange = 20.5f;
    private float spawnRange2 = -20.5f;
    private int enemyCount;
    private int waveNumber = 1;
    private int waveMax = 3;
    void Start()
    {
        SpawnWave(waveNumber);
    }
    private void Update()
    {
        enemyCount = FindObjectsOfType<EnemyFollow>().Length;
        if (enemyCount == 0 && waveNumber < waveMax)
        {
            waveNumber++;
            SpawnWave(waveNumber);
        }
        else if (enemyCount == 0 && waveNumber >= waveMax)
        {
            Debug.Log("End");
        }
    }

    void SpawnWave(int enemyNum)
    {
        Instantiate(speedUpPrefab, GenerateSpawnPosition2(), speedUpPrefab.transform.rotation);
        for (int i = 0; i < enemyNum; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
    Vector3 GenerateSpawnPosition()
    {

        float xPos = Random.Range(-spawnRange, spawnRange);
        float zPos = Random.Range(-spawnRange, spawnRange);
        Vector3 spawnPos = new Vector3(xPos, enemyPrefab.transform.position.y, zPos);
        Debug.Log("Generated Spawn Position: " + spawnPos);
        return spawnPos;
    }

    Vector3 GenerateSpawnPosition2()
    {

        float xPos = Random.Range(-spawnRange2, spawnRange2);
        float zPos = Random.Range(-spawnRange2, spawnRange2);
        Vector3 spawnPos = new Vector3(xPos, enemyPrefab.transform.position.y, zPos);
        Debug.Log("Generated Spawn Position: " + spawnPos);
        return spawnPos;
    }
}