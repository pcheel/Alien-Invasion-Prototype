using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _playerBulletPrefab;
    [SerializeField] private GameObject _enemyBulletPrefab;
    private ObjectPool<GameObject> _playerBulletPool;
    private ObjectPool<GameObject> _enemyBulletPool;
     
    public ObjectPool<GameObject> playerBulletPool
    {
        get { return _playerBulletPool; }
    }
    public ObjectPool<GameObject> enemyBulletPool
    {
        get { return _enemyBulletPool; }
    }

    private void Awake()
    {
        _playerBulletPool = new ObjectPool<GameObject>(
            createFunc: () => CreatePlayerBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => ReturnBullet(bullet),
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: 7,
            maxSize: 7
            );
        _enemyBulletPool = new ObjectPool<GameObject>(
            createFunc: () => CreateEnemyBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => ReturnBullet(bullet),
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 15
            );
    }
    private GameObject CreatePlayerBullet()
    {
        GameObject bullet = Instantiate(_playerBulletPrefab);
        bullet.GetComponent<Bullet>().BulletPool = _playerBulletPool;
        bullet.transform.SetParent(transform);
        return bullet;
    }
    private GameObject CreateEnemyBullet()
    {
        GameObject bullet = Instantiate(_enemyBulletPrefab);
        bullet.GetComponent<Bullet>().BulletPool = _enemyBulletPool;
        bullet.transform.SetParent(transform);
        return bullet;
    }
    private void GetBullet(GameObject bullet)
    {
        bullet.transform.position = transform.position;
        bullet.SetActive(true);
    }
    private void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
