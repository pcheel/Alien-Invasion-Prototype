using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject _playerBulletPrefab;
    [SerializeField] private GameObject _enemyBulletPrefab;
    private ObjectPool<Bullet> _playerBulletPool;
    private ObjectPool<Bullet> _enemyBulletPool;
     
    public ObjectPool<Bullet> playerBulletPool => _playerBulletPool;
    public ObjectPool<Bullet> enemyBulletPool => _enemyBulletPool;

    private void Awake()
    {
        _playerBulletPool = new ObjectPool<Bullet>(
            createFunc: () => CreatePlayerBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => ReturnBullet(bullet),
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: 7,
            maxSize: 7
            );
        _enemyBulletPool = new ObjectPool<Bullet>(
            createFunc: () => CreateEnemyBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => ReturnBullet(bullet),
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 15
            );
    }
    private Bullet CreatePlayerBullet()
    {
        GameObject bulletGO = Instantiate(_playerBulletPrefab);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.bulletPool = _playerBulletPool;
        bulletGO.transform.SetParent(transform);
        return bullet;
    }
    private Bullet CreateEnemyBullet()
    {
        GameObject bulletGO = Instantiate(_enemyBulletPrefab);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.bulletPool = _enemyBulletPool;
        bulletGO.transform.SetParent(transform);
        return bullet;
    }
    private void GetBullet(Bullet bullet)
    {
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
    }
    private void ReturnBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
