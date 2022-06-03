using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private int _damage;
    private float _speed;
    private Vector2 _position;
    private Vector2 _direction;
    private const float LIMIT_POSITION = 40F;
    public ObjectPool<Bullet> bulletPool;

    public int damage => _damage;
    public void StateUpdate(Vector2 direction, Vector2 position, float speed, int damage)
    {
        _speed = speed;
        _direction = direction;
        _position = position;
        _damage = damage;
    }

    private void Update()
    {
        if (Mathf.Abs(_position.x) > LIMIT_POSITION || Mathf.Abs(_position.y) > LIMIT_POSITION)
        {
            bulletPool.Release(this);
            return;
        }
        Move();
    }
    private void Move()
    {
        _position += _direction * _speed * Time.deltaTime;
        transform.position = _position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy _enemy = collision.gameObject.GetComponent<Enemy>();
        Player _player = collision.gameObject.GetComponent<Player>();
        if (_enemy != null || _player != null)
        {
            if (this.gameObject.activeInHierarchy)
            {
                bulletPool.Release(this);
            }
        }
    }
}
