using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _bulletDamage;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private AudioSource _shotSound;
    private ObjectPool<Bullet> _playerBulletPool;
    private int _currentHealth;

    public UnityEvent<int> OnPlayerHealthChanged = new UnityEvent<int>();
    public UnityEvent OnPlayerDied = new UnityEvent();
    public int maxHealth => _maxHealth;
    public void ApplyDamage(int damage)
    {
        if (_currentHealth <= damage)
        {
            _currentHealth = 0;
            OnPlayerDied?.Invoke();
        }
        else
        {
            _currentHealth -= damage;
        }
        OnPlayerHealthChanged?.Invoke(_currentHealth);
    }
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        _playerBulletPool = _bulletPool.playerBulletPool;
    }
    private void Update()
    {
        Move();
        Shot();
    }
    private void Move()
    {
        float directionX = Input.GetAxis("Horizontal");
        float directionY = Input.GetAxis("Vertical");
        Vector3 direction = Vector3.ClampMagnitude(new Vector3(directionX, directionY, 0f), 1f);
        transform.position += direction * _speed * Time.deltaTime;
    }
    private void Shot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(clickPosition.x - transform.position.x, clickPosition.y - transform.position.y);
            direction = Vector2.ClampMagnitude(direction, 1);
            if (_playerBulletPool != null)
            {
                _shotSound.Play();
                Bullet bullet = _playerBulletPool.Get();
                bullet.StateUpdate(direction, transform.position, _bulletSpeed, _bulletDamage); 
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet _bullet = collision.gameObject.GetComponent<Bullet>();
        Enemy _enemy = collision.gameObject.GetComponent<Enemy>();
        if (_bullet != null)
        {
            ApplyDamage(_bullet.damage);
        }
        else if (_enemy != null)
        {
            ApplyDamage(_enemy.hitData._collisionDamage);
        }
    }
}
