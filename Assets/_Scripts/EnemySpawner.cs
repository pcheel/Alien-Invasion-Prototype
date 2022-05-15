using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int _maxEnemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private float _spawnRange;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Player _player;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private AudioSource _enemyDiedSound;
    [SerializeField] private AudioSource _enemyShotSound;
    [SerializeField] private GameObject[] _spawnPoint;
    [SerializeField] private Color[] _enemyColors;
    [SerializeField] private Sprite[] _enemySprites;
    [SerializeField] private EnemyMoveData[] _moveData;
    [SerializeField] private EnemyHitData[] _hitData;

    private ObjectPool<GameObject> _enemyBulletPool;
    private ObjectPool<GameObject> _enemyPool;
    private float _timeFromLastSpawn = 0;
    private int _enemyCounter = 0;
    private Dictionary<int, Func<IEnemyDirectionSetter>> _enemyFormSpawnActions;
    private Dictionary<int, Func<IEnemyHitSetter>> _enemyColorSpawnActions;

    private void Awake()
    {
        _enemyFormSpawnActions = new Dictionary<int, Func<IEnemyDirectionSetter>>()
        {
            {0, () => CubeSpawn() },
            {1, () => RhombusSpawn() },
            {2, () => TriangleSpawn() }
        };
        _enemyColorSpawnActions = new Dictionary<int, Func<IEnemyHitSetter>>()
        {
            {0, () => RedSpawn() },
            {1, () => YellowSpawn() }
        };
        _enemyPool = new ObjectPool<GameObject>(
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => GetEnemy(enemy),
            actionOnRelease: (enemy) => ReturnEnemy(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 10
        );
    }
    private void Start()
    {
        _enemyBulletPool = _bulletPool.enemyBulletPool;
        EventManager.OnEnemyDied.AddListener(() => _enemyDiedSound.Play());
    }
    private void Update()
    {
        SpawnEnemy();
    }
    private void SpawnEnemy()
    {
        if (_enemyCounter < _maxEnemy && _timeFromLastSpawn >= _spawnTime)
        {
            GameObject enemy = _enemyPool.Get();
            _enemyCounter++;
            _timeFromLastSpawn = 0;
        }
        else
        {
            _timeFromLastSpawn += Time.deltaTime;
        }
    }
    private GameObject CreateEnemy()
    {
        GameObject enemyGO = Instantiate(_enemyPrefab, RandomizeSpawnPoint());
        enemyGO.transform.SetParent(transform);
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        enemy.player = _player;
        enemy.enemyPool = _enemyPool;
        enemy.bulletPool = _enemyBulletPool;
        enemy.enemyShotSound = _enemyShotSound;
        return enemyGO;
    }
    private void GetEnemy(GameObject enemy)
    {
        RandomizeEnemy(enemy);
        Transform spawnTransform = RandomizeSpawnPoint();
        float spawnPositionX = spawnTransform.position.x + UnityEngine.Random.Range(0f, _spawnRange);
        float spawnPositionY = spawnTransform.position.y + UnityEngine.Random.Range(0f, _spawnRange);
        enemy.GetComponent<Enemy>().StateUpdate(new Vector2(spawnPositionX, spawnPositionY));
        enemy.SetActive(true);
    }
    private void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        _enemyCounter--;
    }
    private void RandomizeEnemy(GameObject enemyGO)
    {
        Enemy enemy = enemyGO.GetComponent<Enemy>();
        int formNumber = UnityEngine.Random.Range(0, _enemyFormSpawnActions.Count);
        int colorNumber = UnityEngine.Random.Range(0, _enemyColorSpawnActions.Count);
        enemy.directionSetter = _enemyFormSpawnActions[formNumber]?.Invoke();
        enemy.hitSetter = _enemyColorSpawnActions[colorNumber]?.Invoke();
        enemy.gameObject.GetComponent<SpriteRenderer>().sprite = _enemySprites[formNumber];
        enemy.gameObject.GetComponent<SpriteRenderer>().color = _enemyColors[colorNumber];
        enemy.moveData = _moveData[formNumber];
        enemy.hitData = _hitData[colorNumber];
    }
    private Transform RandomizeSpawnPoint()
    {
        GameObject spawnPoint = _spawnPoint[UnityEngine.Random.Range(0, _spawnPoint.Length)];
        return spawnPoint.transform;
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
    private IEnemyHitSetter RedSpawn()
    {
        return new RedHitSetter();
    }
    private IEnemyHitSetter YellowSpawn()
    {
        return new YellowHitSetter();
    }
}
