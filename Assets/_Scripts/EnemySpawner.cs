using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private ObjectPool _pool;

    private float _timeFromLastSpawn = 0;
    private int _enemyCounter = 0;

    private void Update()
    {
        if (_enemyCounter < _maxEnemy && _timeFromLastSpawn >= _spawnTime)
        {
            SpawnEnemy();
        }
        else
        {
            _timeFromLastSpawn += Time.deltaTime;
        }
    }
    private void SpawnEnemy()
    {
        GameObject enemy = _pool.GetObject();
        enemy.GetComponent<Enemy>().OnDied += () => _enemyCounter--;
        enemy.transform.position = transform.position;
        _enemyCounter++;
        _timeFromLastSpawn = 0;
    }
}
