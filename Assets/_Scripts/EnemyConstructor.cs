using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class EnemyConstructor : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private AudioSource _enemyShotSound;
    [SerializeField] private EnemyMoveData[] _moveData;
    [SerializeField] private EnemyHitData[] _hitData;
    [SerializeField] private Color[] _enemyColors;
    [SerializeField] private Sprite[] _enemySprites;

    private ObjectPool<Bullet> _enemyBulletPool;
    private Dictionary<int, Func<IEnemyDirectionSetter>> _enemyFormSpawnActions;
    private Dictionary<int, Func<IEnemyHitSetter>> _enemyColorSpawnActions;
    
    public void InitializeEnemy(Enemy enemy)
    {
        enemy.player = _player;
        enemy.bulletPool = _enemyBulletPool;
        enemy.enemyShotSound = _enemyShotSound;
    }
    public void RandomizeEnemy(Enemy enemy)
    {
        int formNumber = UnityEngine.Random.Range(0, _enemyFormSpawnActions.Count);
        int colorNumber = UnityEngine.Random.Range(0, _enemyColorSpawnActions.Count);
        enemy.directionSetter = _enemyFormSpawnActions[formNumber]?.Invoke();
        enemy.hitSetter = _enemyColorSpawnActions[colorNumber]?.Invoke();
        enemy.gameObject.GetComponent<SpriteRenderer>().sprite = _enemySprites[formNumber];
        enemy.gameObject.GetComponent<SpriteRenderer>().color = _enemyColors[colorNumber];
        enemy.moveData = _moveData[formNumber];
        enemy.hitData = _hitData[colorNumber];
    }

    private void Awake()
    {
        _enemyBulletPool = _bulletPool.enemyBulletPool;
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
