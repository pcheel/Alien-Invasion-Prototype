using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shotDelay;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private GameObject _bulletPrefab;
    private ObjectPool<GameObject> _bulletPool;
    private float _currentHealth;
    private float _timeFromLastShot = 0;
    public static Player _player;

    private void Awake()
    {
        _player = this;
        _currentHealth = _maxHealth;
        _bulletPool = new ObjectPool<GameObject>(
            createFunc: () => CreateBullet(),
            actionOnGet: (bullet) => GetBullet(bullet),
            actionOnRelease: (bullet) => ReturnBullet(bullet),
            actionOnDestroy: (bullet) => Destroy(bullet),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 5
            );
    }
    private void Update()
    {
        _timeFromLastShot += Time.deltaTime;
        Move();
        if (Input.GetKey(KeyCode.Mouse0) && _shotDelay <= _timeFromLastShot)
        {
            _timeFromLastShot = 0f;
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(click.x - transform.position.x, click.y - transform.position.y);
            Shoot(direction);
        }
    }
    private void Move()
    {
        float directionX = Input.GetAxis("Horizontal");
        float directionY = Input.GetAxis("Vertical");
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(directionX, directionY, 0f), 1f);
        transform.position += direction * _speed * Time.deltaTime;
    }
    public void ApplyDamage(int damage)
    {
        if (_currentHealth < damage)
        {
            _currentHealth = 0;
        }
        else
        {
            _currentHealth -= damage;
        }
        Debug.Log($"Вам нанесли {damage} урона");
        Debug.Log($"_currentHealth = {_currentHealth}");
    }
    private void Shoot(Vector2 direction)
    {
        GameObject bullet = _bulletPool.Get();
        bullet.GetComponent<Bullet>().StateUpdate(direction, _bulletSpeed, transform.position, _bulletDamage);   
    }
    private void GetBullet(GameObject bullet)
    {
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
    }
    private GameObject CreateBullet()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.GetComponent<Bullet>().BulletPool = _bulletPool;
        bullet.transform.SetParent(transform);
        return bullet;
    }
    private void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
