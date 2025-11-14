using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner
{
    private readonly ObjectPool<Enemy> _pool;
    private readonly Tilemap _tilemapSpawnArea;
    private readonly LayerMask _enemyLayer; 

    private const int MaxAttempts = 20; 

    public EnemySpawner(ObjectPool<Enemy> pool, Tilemap tilemap, LayerMask enemyLayer)
    {
        _pool = pool;
        _tilemapSpawnArea = tilemap;
        _enemyLayer = enemyLayer;
    }


    public List<Enemy> SpawnEnemies(int count)
    {
        List<Enemy> enemies = new();

        var spawnablePositions = new List<Vector3Int>();
        foreach (var pos in _tilemapSpawnArea.cellBounds.allPositionsWithin)
        {
            if (_tilemapSpawnArea.HasTile(pos))
                spawnablePositions.Add(pos);
        }

        for (int i = 0; i < count; i++)
        {
            Enemy e = _pool.Get();
            Vector3 spawnPos = GetRandomFreePosition(spawnablePositions, e.transform);

            e.transform.position = spawnPos;
            enemies.Add(e);
        }

        return enemies;
    }

    private Vector3 GetRandomFreePosition(List<Vector3Int> spawnablePositions, Transform enemyTransform)
    {
        Vector3 finalPos = Vector3.zero;
        int attempts = 0;

        do
        {
            Vector3Int cellPos = spawnablePositions[Random.Range(0, spawnablePositions.Count)];
            finalPos = _tilemapSpawnArea.GetCellCenterWorld(cellPos);

            attempts++;
            // проверяем, есть ли на позиции другой враг
        } while (Physics2D.OverlapCircle(finalPos, 3, _enemyLayer) && attempts < MaxAttempts);

        return finalPos;
    }
}