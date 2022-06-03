using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private int _currentHealth;
    private float _timeFromLastShot = 0f;
    private Vector3 _position;
    private Player _player;
    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<Bullet> _enemyBulletPool;
    private AudioSource _enemyShotSound;
    private IEnemyDirectionSetter _directionSetter;
    private IEnemyHitSetter _hitSetter;

    public Player player { set => _player = value;}
    public AudioSource enemyShotSound { set => _enemyShotSound = value;}
    public ObjectPool<Enemy> enemyPool{set => _enemyPool = value; }
    public ObjectPool<Bullet> bulletPool{set => _enemyBulletPool = value; }
    public IEnemyDirectionSetter directionSetter{set => _directionSetter = value;}
    public IEnemyHitSetter hitSetter{set => _hitSetter = value;}

    public EnemyMoveData moveData;
    public EnemyHitData hitData;

    public void StateUpdate(Vector2 position)
    {
        _currentHealth = moveData._maxHealth;
        _position = position;
        transform.position = position;
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
        _position += direction * moveData._speed * Time.deltaTime;
        transform.position = _position;
    }
    private void Hit()
    {
        if (_timeFromLastShot >= hitData._shotDelay && hitData._shotDelay > 0f)
        {
            _hitSetter.Hit(_enemyBulletPool, transform.position, hitData);
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
            ApplyDamage(bullet.damage);
        }
        else if(player != null)
        {
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
        _enemyPool.Release(this);
    }
}
