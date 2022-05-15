using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    private int _score;
    private float _timeFromLastShot = 0f;
    private Vector3 _position;
    private Player _player;
    private ObjectPool<GameObject> _enemyPool;
    private ObjectPool<GameObject> _enemyBulletPool;
    private AudioSource _enemyShotSound;
    private EnemyMoveData _moveData;
    private EnemyHitData _hitData;
    private IEnemyDirectionSetter _directionSetter;
    private IEnemyHitSetter _hitSetter;

    public Player player{ set { _player = value; }}
    public ObjectPool<GameObject> enemyPool{set { _enemyPool = value; }}
    public ObjectPool<GameObject> bulletPool{set { _enemyBulletPool = value; }}
    public EnemyHitData hitData{set { _hitData = value; }}
    public EnemyMoveData moveData{set { _moveData = value; }}
    public AudioSource enemyShotSound{set {_enemyShotSound = value; }}
    public IEnemyDirectionSetter directionSetter{set {_directionSetter = value;}}
    public IEnemyHitSetter hitSetter{set {_hitSetter = value;}}

    public void StateUpdate(Vector2 position)
    {
        _currentHealth = _moveData._maxHealth;
        _position = position;
        transform.position = position;
        CalculateScore();
    }
    private void Awake()
    {
        _position = transform.position;
    }
    private void Update()
    {
        Move();
        Hit();
    }
    private void Move()
    {
        Vector3 direction = _directionSetter.GetDirection(_position, _player.transform.position);
        direction = Vector3.ClampMagnitude(direction, 1);
        _position += direction * _moveData._speed * Time.deltaTime;
        transform.position = _position;
    }
    private void Hit()
    {
        if (_timeFromLastShot >= _hitData._shotDelay && _hitData._shotDelay > 0f)
        {
            _hitSetter.Hit(_enemyBulletPool, transform.position, _hitData);
            _timeFromLastShot = 0f;
            _enemyShotSound.Play();
        }
        else
        {
            _timeFromLastShot += Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            ApplyDamage(bullet.Damage);
            if (bullet.gameObject.activeInHierarchy)
            {
                bullet.BulletPool.Release(bullet.gameObject);
            }
        }
        else if(player != null)
        {
            EventManager.SendPlayerDamageApplied(_hitData._collisionDamage);
            Die();
        }
    }
    private void ApplyDamage(int damage)
    {
        if (damage < _currentHealth)
        {
            _currentHealth -= damage;
        }
        else
        {
            Die();
        }
    }
    private void Die()
    {
        _enemyPool.Release(gameObject);
        EventManager.SendEnemyDied(_score);
    }
    private void CalculateScore()
    {
        _score = _hitData._hitScore + _moveData._moveScore;
    }
}
