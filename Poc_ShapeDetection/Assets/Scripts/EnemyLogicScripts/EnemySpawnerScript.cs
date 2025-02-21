using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn settings")]
    [Tooltip("Rate at which enemies are spawned (in seconds)")]
    public float spawnRate = 1f;

    [Tooltip("If true, the spawner will spawn enemies continuously")]
    public bool spawnContinuously = false;

    void Start()
    {
        InstantiateEnemy();
    }

    void Update()
    {
        if (spawnContinuously)
        {
            InvokeRepeating("InstantiateEnemy", 0, spawnRate);
        }
    }

    void InstantiateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.transform.parent = transform;
    }

}
