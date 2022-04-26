using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject[] _enemyPrefabs;

    private ObjectPool<GameObject> _pool;
    private float _timeFromLastSpawn = 0;
    private int _enemyCounter = 0;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => GetEnemy(enemy),
            actionOnRelease: (enemy) => ReturnEnemy(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 10
            );
    }
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
        GameObject enemy = _pool.Get();
        enemy.GetComponent<Enemy>().OnDied += () => _enemyCounter--;
        enemy.transform.position = transform.position;
        _enemyCounter++;
        _timeFromLastSpawn = 0;
    }

    private GameObject CreateEnemy()
    {
        int prefabNumber = Random.Range(0, _enemyPrefabs.Length);
        GameObject enemy = Instantiate(_enemyPrefabs[prefabNumber]);
        enemy.transform.SetParent(this.transform);
        enemy.GetComponent<Enemy>().pool = _pool;
        return enemy;
    }
    private void GetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
    }
    private void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
    }
}
