using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Player _player;
    [SerializeField] private Color[] _enemyColors;
    [SerializeField] private Sprite[] _enemySprites;

    private ObjectPool<GameObject> _pool;
    private float _timeFromLastSpawn = 0;
    private int _enemyCounter = 0;
    private Dictionary<int, Func<IEnemyDirectionSetter>> _enemyFormSpawnActions;

    private void Awake()
    {
        _enemyFormSpawnActions = new Dictionary<int, Func<IEnemyDirectionSetter>>()
        {
            {0, () => CubeSpawn() },
            {1, () => RhombusSpawn() },
            {2, () => TriangleSpawn() }
        };
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
        enemy.transform.position = transform.position;
        _enemyCounter++;
        _timeFromLastSpawn = 0;
    }
    private GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab, transform);
        enemy.GetComponent<Enemy>().player = _player;
        enemy.GetComponent<Enemy>().pool = _pool;
        enemy.transform.SetParent(this.transform);
        return enemy;
    }
    private void GetEnemy(GameObject enemy)
    {
        enemy.SetActive(true);
        RandomizeEnemy(enemy);
        enemy.GetComponent<Enemy>().StateUpdate();
    }
    private void ReturnEnemy(GameObject enemy)
    {
        enemy.transform.position = transform.position;
        enemy.SetActive(false);
        _enemyCounter--;
    }
    private void RandomizeEnemy(GameObject enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        int number = UnityEngine.Random.Range(0, _enemyFormSpawnActions.Count);
        enemy._directionSetter = _enemyFormSpawnActions[number]?.Invoke();
        enemy.gameObject.GetComponent<SpriteRenderer>().sprite = _enemySprites[number];
    }
    private IEnemyDirectionSetter CubeSpawn()
    {
        return new CubeDirectionSetter();
    }
    private IEnemyDirectionSetter RhombusSpawn()
    {
        return new RhombusDirectionSetter();
    }
    private IEnemyDirectionSetter TriangleSpawn()
    {
        return new TriangleDirectionSetter();
    }
}
