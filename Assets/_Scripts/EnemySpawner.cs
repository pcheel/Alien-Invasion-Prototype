using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnRange;
    [SerializeField] private AudioSource _enemyDiedSound;
    [SerializeField] private GameObject[] _spawnPoint;

    private ObjectPool<Enemy> _enemyPool;
    private float _timeFromLastSpawn = 0;
    private int _enemyCounter = 0;

    public void RelocateEnemy(Enemy enemy)
    {
        Transform spawnTransform = RandomizeSpawnPoint();
        float spawnPositionX = spawnTransform.position.x + UnityEngine.Random.Range(0f, _spawnRange);
        float spawnPositionY = spawnTransform.position.y + UnityEngine.Random.Range(0f, _spawnRange);
        enemy.StateUpdate(new Vector2(spawnPositionX, spawnPositionY));
    }
    private void Start()
    {
        EventManager.OnEnemyDied.AddListener(() => _enemyDiedSound.Play());
        EventManager.OnEnemyDied.AddListener(() => _enemyCounter--);
        _enemyPool = GetComponent<EnemyPool>().enemyPool;
    }
    private void Update()
    {
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        if (_enemyCounter < _maxEnemy && _timeFromLastSpawn >= _spawnTime)
        {
            Enemy enemy = _enemyPool.Get();
            _enemyCounter++;
            _timeFromLastSpawn = 0;
        }
        else
        {
            _timeFromLastSpawn += Time.deltaTime;
        }
    }
    private Transform RandomizeSpawnPoint()
    {
        GameObject spawnPoint = _spawnPoint[UnityEngine.Random.Range(0, _spawnPoint.Length)];
        return spawnPoint.transform;
    }
}
