using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private ScoreCounter _scoreCounter;

    private EnemyConstructor _enemyConstructor;
    private ObjectPool<Enemy> _enemyPool;
    private EnemySpawner _enemySpawner;
    
    public ObjectPool<Enemy> enemyPool => _enemyPool;
    public UnityEvent OnEnemyDied = new UnityEvent();

    private void Awake()
    {
        _enemyConstructor = GetComponent<EnemyConstructor>();
        _enemySpawner = GetComponent<EnemySpawner>();
        _enemyPool = new ObjectPool<Enemy>(
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => GetEnemy(enemy),
            actionOnRelease: (enemy) => ReturnEnemy(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 10
        );
    }
    private Enemy CreateEnemy()
    {
        GameObject enemyGO = Instantiate(_enemyPrefab);
        enemyGO.transform.SetParent(transform);
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        enemy.enemyPool = _enemyPool;
        _enemyConstructor.InitializeEnemy(enemy);
        _enemyConstructor.RandomizeEnemy(enemy);
        return enemy;
    }
    private void GetEnemy(Enemy enemy)
    {
        _enemyConstructor.RandomizeEnemy(enemy);
        _enemySpawner.RelocateEnemy(enemy);
        enemy.gameObject.SetActive(true);
    }
    private void ReturnEnemy(Enemy enemy)
    {
        _scoreCounter.ChangeScore(enemy.moveData._moveScore + enemy.hitData._hitScore);
        OnEnemyDied?.Invoke();
        enemy.gameObject.SetActive(false);
    }
}
