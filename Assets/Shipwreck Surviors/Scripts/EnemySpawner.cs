using System.Collections;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to your enemy prefab
    public float spawnRadius = 10f; // Initial spawn radius
    public float timeBetweenWaves = 10f; // Time between waves
    public int baseEnemiesPerWave = 5; // Initial number of enemies per wave
    public int enemiesPerWaveIncrease = 2; // Increase in enemies per wave

    private Transform playerTransform;

    void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return null; // Wait for one frame
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming your player has the "Player" tag
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        int currentEnemiesPerWave = baseEnemiesPerWave;

        while (true) // Infinite loop for continuous spawning
        {
            SpawnWave(currentEnemiesPerWave);
            yield return new WaitForSeconds(timeBetweenWaves);
            currentEnemiesPerWave += enemiesPerWaveIncrease; // Increase enemies per wave for the next wave
        }
    }

    void SpawnWave(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPosition = RandomCircle(playerTransform.position, spawnRadius);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector2 RandomCircle(Vector2 center, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = center.y + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }
}
