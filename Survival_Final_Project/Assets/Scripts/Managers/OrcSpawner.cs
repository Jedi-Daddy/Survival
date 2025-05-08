using UnityEngine;
using System.Collections.Generic;

public class OrcSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject orcPrefab;
    public float spawnInterval = 5f;
    public int maxOrcs = 10;
    public float spawnRadius = 50f;

    private List<GameObject> spawnedOrcs = new List<GameObject>();
    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            TrySpawnOrc();
        }
    }

    void TrySpawnOrc()
    {
        spawnedOrcs.RemoveAll(item => item == null);

        if (spawnedOrcs.Count >= maxOrcs)
        {
            return;
        }

        Vector3 spawnPosition = GetRandomPosition();
        GameObject newOrc = Instantiate(orcPrefab, spawnPosition, Quaternion.identity);
        spawnedOrcs.Add(newOrc);
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomPoint = Random.insideUnitSphere * spawnRadius;
        randomPoint.y = 0;
        return transform.position + randomPoint;
    }
}
