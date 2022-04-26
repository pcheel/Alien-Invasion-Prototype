using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Pool;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed;
    [SerializeField] protected int _damage;
    [SerializeField] protected int _maxHealth;
    protected ObjectPool<GameObject> _pool;
    protected EnemyFactory _factory;
    protected EnemyMovement _movement;
    protected EnemyHit _hit;
    protected int _currentHealth;
    public Action OnDied;

    protected void Awake()
    {
        _currentHealth = _maxHealth;
    }
    protected void Die()
    {
        OnDied?.Invoke();
        _pool.Release(this.gameObject);
    }
    protected void ApplyDamage(int damage)
    {
        if (damage >= _currentHealth)
        {
            _currentHealth = 0;
            Die();
        }
        else
        {
            _currentHealth -= damage;
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            ApplyDamage(bullet.Damage);
            bullet.Die();
        }
        else if (collision.gameObject.GetComponent<Player>() != null)
        {
            _hit.Hit(_damage);
            Die();
        }
    }
    public ObjectPool<GameObject> pool
    {
        set
        {
            _pool = value;
        }
    }
}
