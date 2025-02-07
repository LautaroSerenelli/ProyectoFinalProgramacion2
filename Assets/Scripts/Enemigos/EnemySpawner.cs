using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public Vector3 spawnPositionOffset;
    }

    public EnemySpawnInfo[] enemiesToSpawn;
    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnEnemies();
            hasSpawned = true;
        }
    }

    private void SpawnEnemies()
    {
        foreach (EnemySpawnInfo spawnData in enemiesToSpawn)
        {
            if (spawnData.enemyPrefab != null && spawnData.spawnPositionOffset != null)
            {
                Instantiate(spawnData.enemyPrefab, spawnData.spawnPositionOffset, Quaternion.identity);
            }
        }
    }
}