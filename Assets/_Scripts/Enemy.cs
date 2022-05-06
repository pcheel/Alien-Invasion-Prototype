using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData _data;
    private int _currentHealth;
    private Vector3 _position;
    private Player _player;
    private ObjectPool<GameObject> _pool;
    public IEnemyDirectionSetter _directionSetter;

    public Player player
    {
        set { _player = value; }
    }
    public ObjectPool<GameObject> pool
    {
        set { _pool = value; }
    }
    public void StateUpdate()
    {
        _currentHealth = _data._maxHealth;
        _position = transform.position;
    }
    private void Awake()
    {
        _position = transform.position;
        StateUpdate();
        _directionSetter = new CubeDirectionSetter();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        Vector3 direction = _directionSetter.GetDirection(_position, _player.transform.position);
        _position += direction * _data._speed * Time.deltaTime;
        transform.position = _position;
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
            player.ApplyDamage(_data._damage);
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
        _pool.Release(gameObject);
    }
}
