using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Tilemap _spawnArea;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private LayerMask _enemyLayer;

    private ObjectPool<Enemy> _pool;
    private EnemySpawner _spawner;
    private List<Enemy> _activeEnemies = new();

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(_enemyPrefab, 10, this.gameObject.transform);
        _spawner = new EnemySpawner(_pool, _spawnArea, _enemyLayer);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<DiedEvent>(OnEnemyDie);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<DiedEvent>(OnEnemyDie);
    }
    public void SpawnEnemies(int spawnCount)
    {
        var spawned = _spawner.SpawnEnemies(spawnCount);
        _activeEnemies.AddRange(spawned);
    }

    public void RemoveEnemies()
    {
        foreach (var enemy in _activeEnemies)
        {
            if (enemy == null) continue;
            enemy.gameObject.SetActive(false); 
            _pool.ReturnToPool(enemy);
        }

        _activeEnemies.Clear();
    }
    void OnEnemyDie(DiedEvent e)
    {
        if (e.owner == null) return;

        if (e.owner.TryGetComponent<Enemy>(out var enemy))
        {
            _activeEnemies.Remove(enemy);

            enemy.gameObject.SetActive(false);
            _pool.ReturnToPool(enemy);

            if (_activeEnemies.Count == 0)
            {
                print("no enemies left");
                EventBus.Publish(new AllEnemiesDeadEvent());
            }
        }
    }
}