using System.Collections;
using UnityEngine;
using Pathfinding;

public class SpawnManager : MonoBehaviour
{
    public GameObject basicEnemyPrefab;
    public GameObject normalEnemyPrefab;
    public float spawnInterval = 5f;
    public int maxEnemies = 10;
    public float spawnRadius = 20f; // Radius around the SpawnManager to consider for spawning

    private int currentEnemyCount = 0;
    private Coroutine spawnCoroutine;

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemiesCoroutine());
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                Vector2 spawnPoint;
                if (TryGetRandomWalkablePoint(out spawnPoint))
                {
                    SpawnEnemy(spawnPoint);
                    currentEnemyCount++;
                }
            }
        }
    }

    bool TryGetRandomWalkablePoint(out Vector2 result)
    {
        for (int i = 0; i < 10; i++) // Try several times to find a walkable point
        {
            Vector2 randomPoint = (Random.insideUnitCircle * spawnRadius) + (Vector2)PlayerController.Instance.gameObject.transform.position;
            if (IsValidSpawnPoint(randomPoint))
            {
                result = randomPoint;
                return true;
            }
        }

        result = Vector2.zero;
        return false;
    }

    bool IsValidSpawnPoint(Vector2 point)
    {
        // Check if the point is on a walkable node
        GraphNode node = AstarPath.active.data.graphs[0].GetNearest(point).node;
        return node != null && node.Walkable;
    }

    void SpawnEnemy(Vector2 spawnPoint)
    {
        GameObject enemyPrefab = Random.Range(0, 4) > 0 ? basicEnemyPrefab : normalEnemyPrefab;
        Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
    }

    public void OnEnemyDeath()
    {
        currentEnemyCount--;
    }
}
