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

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask wallLayerMask;

    public GameObject particleSystemPrefab;

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
        if (node == null || !node.Walkable) return false;

        // Perform a raycast to check the surface type
        RaycastHit2D hit = Physics2D.Raycast(point, Vector2.up, 1f, groundLayerMask);
        Debug.DrawRay(point, Vector2.up, Color.red, 1f);

        if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Ensure it's not too close to walls
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 2f, wallLayerMask);
            if (colliders.Length == 0)
            {
                return true;
            }
        }

        return false;
    }


    void SpawnEnemy(Vector2 spawnPoint)
    {
        GameObject particleSystemObject = Instantiate(particleSystemPrefab, spawnPoint, Quaternion.identity);
        ParticleSystem particleSystem = particleSystemObject.GetComponent<ParticleSystem>();

        // Subscribe to the OnParticleSystemStopped event
        ParticleSystemStoppedListener listener = particleSystemObject.AddComponent<ParticleSystemStoppedListener>();
        listener.Initialize(particleSystem);

        // Spawn enemy after the particle system stops
        listener.OnParticleSystemStoppedCallback += () =>
        {
            GameObject enemyPrefab = Random.Range(0, 4) > 0 ? basicEnemyPrefab : normalEnemyPrefab;
            Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        };
    }

    public void OnEnemyDeath()
    {
        currentEnemyCount--;
    }
}
